using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Match3Engine
{
    public class M3GamePlay
    {
        private M3Board mBoard;
        private Dictionary<int, M3Figure> mIds;
        private int mId;
        
        public enum eMode { CHECK_STATE, PERFORMING_COMBOS, INACTION, INACTION_BY_TIMER, COMPLETENESS };

        public struct LevelInfo
        {
            public LevelInfo(int coins, int steps)
            {
                this.coins = coins;
                this.steps = steps;
            }
            
            public int coins;
            public int steps;
        }

        private eMode mMode;
        private const uint TIMER_VALUE = 2;
        private uint mTimer = TIMER_VALUE;
        private List<M3Combo> mCurrentCombos;
        private M3Figure.eType mCurrentGiftType;
        private M3Position mCurrentGiftPosition;
        private M3Figure.eColor mRainbowColor;
        private M3Position mStepBeginPos;
        private M3Position mStepEndPos;
        private List<M3Position> mVoidsForReshuffle;
        private List<LevelInfo> mLevels;

        private bool mNeedLogStep = true;

        public struct DropInfo
        {
            public enum eDirection { DOWN, LEFT, RIGHT };
            public M3Position pos;
            public eDirection dir;

            public DropInfo(M3Position pos, eDirection dir)
            {
                this.pos = pos;
                this.dir = dir;
            }
        }

        public M3GamePlay(int width, int height)
        {
            mBoard = new M3Board(width, height);
            mIds = new Dictionary<int, M3Figure>();
            mId = 0;
            mCurrentCombos = new List<M3Combo>();
            mCurrentGiftType = M3Figure.eType.NONE;
            mCurrentGiftPosition = new M3Position();
            mRainbowColor = M3Figure.eColor.NONE;    
            mStepBeginPos = new M3Position();
            mStepEndPos = new M3Position();
            mVoidsForReshuffle = new List<M3Position>();
            mLevels = new List<LevelInfo>();
            mMode = eMode.INACTION;
        }

        public List<LevelInfo> GetLevelsInfo()
        {
            return mLevels;
        }

        private string GetConfigData(string fileName)
        {
            var configPath = Path.Combine(Application.streamingAssetsPath + "/", fileName);
            string data;
            
#if UNITY_EDITOR || UNITY_IOS
            data = File.ReadAllText(configPath);
#elif UNITY_ANDROID
            WWW reader = new WWW (configPath);
            while (!reader.isDone) {}
            data = reader.text;
#endif
            return data;
        }

        private Dictionary<M3Position, M3Cell.eState> LoadXmlConfig(int level)
        {
            var frames = new Dictionary<M3Position, M3Cell.eState>();
            var board = XElement.Parse(GetConfigData("frames.xml"));

            mLevels.Clear();
            var curLevel = board.FirstNode;
            while (curLevel != null)
            {
                var nodeLevel = (XElement) curLevel;

                var stepsAttribute = nodeLevel.FirstAttribute.NextAttribute;
                var coinsAttribute = stepsAttribute.NextAttribute;
                    
                var steps = Convert.ToInt32(stepsAttribute.Value);
                var coins = Convert.ToInt32(coinsAttribute.Value);
                    
                mLevels.Add(new LevelInfo(coins, steps));

                if (level != Convert.ToInt32(nodeLevel.FirstAttribute.Value))
                {
                    curLevel = curLevel.NextNode;
                    continue;
                }

                var pos = nodeLevel.FirstNode;
                while (pos != null)
                {
                    var nodePos = (XElement) pos;
                    var attr = nodePos.FirstAttribute;
                    
                    var posX = Convert.ToInt32(attr.Value);
                    attr = attr.NextAttribute;
                    var posY = Convert.ToInt32(attr.Value);
                    attr = attr.NextAttribute;
                    var type = attr.Value;

                    var cellState = M3Cell.eState.NONE;
                    switch (type)
                    {
                        case "frame":
                            cellState = M3Cell.eState.FRAME;
                            break;
                        case "chain":
                            cellState = M3Cell.eState.CHAIN;
                            break;
                    }

                    frames.Add(new M3Position(posX, posY), cellState);
                    
                    pos = pos.NextNode;
                }
                
                curLevel = curLevel.NextNode;
            }

            return frames;
        }
        
        private int GetNextId()
        {
            return ++mId;
        }

        public void SetEngineMode(eMode mode) { mMode = mode; }

        public void CreateObject(M3Figure.eType type, M3Figure.eColor color, M3Position position)
        {
            M3Settings.CreateObject(type, color, position);

            var id = GetNextId();
            mBoard.FillCell(position, id);
            mIds.Add(id, new M3Figure(type, color));
        }

        public void DestroyObject(M3Position position)
        {
            M3Settings.DestroyObject(position);
            var id = mBoard.GetCellByPos(position).FigureId();
            mIds.Remove(id);
            mBoard.EmptyCell(position);
        }

        public void DestroyAllObjects()
        {
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var pos = new M3Position(x, y);

                    if (!mBoard.GetCellByPos(pos).IsEmpty())
                        DestroyObject(pos);
                }
            }
        }

        public void MoveObjects(M3Position pos1, M3Position pos2)
        {
            M3Settings.SetAction(M3Settings.eState.MOVE, pos1, pos2);
            M3Settings.SetAction(M3Settings.eState.MOVE, pos2, pos1);
            
            M3Settings.ActionFunction delegate1 = M3Settings.GetActionDelegate(pos1);
            M3Settings.ActionFunction delegate2 = M3Settings.GetActionDelegate(pos2);
            
            M3Settings.SetActionDelegate(pos1, delegate2);
            M3Settings.SetActionDelegate(pos2, delegate1);
            
            mBoard.SwapCells(pos1, pos2);
        }

        public void DropObject(M3Position pos, M3Position target)
        {
            M3Settings.SetAction(M3Settings.eState.FALL, pos, target);
            M3Settings.SetActionDelegate(target, M3Settings.GetActionDelegate(pos));
            M3Settings.SetActionDelegate(pos, null);

            var cell = mBoard.GetCellByPos(pos);
            mBoard.FillCell(target, cell.FigureId());
            mBoard.EmptyCell(pos);
        }

        public void CreateFrameObject(M3Cell.eState state, M3Position pos)
        {
            M3Settings.CreateFrameObject(state, pos);
            mBoard.GetCellByPos(pos).SetState(state);
        }
        
        public void DestroyFrameObject(M3Position position, M3Cell.eState state)
        {
            M3Settings.DestroyFrameObject(position, state);
            
            if (state == M3Cell.eState.FRAME)
                mBoard.GetCellByPos(position).SetFreeState();
            else if (state == M3Cell.eState.CHAIN)
                mBoard.GetCellByPos(position).SetFrameState();
        }
        
        public void ShuffleFigures(List<M3Position> exceptPos = null)
        {
            var rand = new Random();
            
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var position = new M3Position(x, y);

                    if (exceptPos != null && exceptPos.Contains(position))
                        continue;

                    var color = (M3Figure.eColor) rand.Next(1, 6);
                    CreateObject(M3Figure.eType.NORMAL, color, position);
                }
            }
        }
        
        public void FindVoids(ref List<M3Position> voids)
        {
            voids.Clear();
            
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var pos = new M3Position(x, y);

                    if (mBoard.GetCellByPos(pos).IsEmpty())
                        voids.Add(pos);
                }
            }
        }

        public void LoadFrames(int level)
        {
            var frames = LoadXmlConfig(level);

            M3Settings.DebugLog("M3: Load xml config");
            
            foreach (var frame in frames)
            {
                CreateFrameObject(frame.Value, frame.Key);
            }
            
            mMode = eMode.CHECK_STATE;
        }

        private bool IsStepAvailable(M3Position begin, M3Position end)
        {
            int x1 = begin.X();
            int y1 = begin.Y();

            int x2 = end.X();
            int y2 = end.Y();
            
            bool isNeighbourhood = ((x1 == x2 && (y1 + 1 == y2 || y1 - 1 == y2)) || (y1 == y2 && (x1 + 1 == x2 || x1 - 1 == x2)));
            bool isBeginFree = mBoard.GetCellByPos(begin).State() != M3Cell.eState.CHAIN;
            bool isEndFree = mBoard.GetCellByPos(end).State() != M3Cell.eState.CHAIN;

            return isNeighbourhood && isBeginFree && isEndFree;
        }

        public void Move(M3Position begin, M3Position end)
        { 
            if (!IsStepAvailable(begin, end))
                return;

            var figure = mIds[mBoard.GetCellByPos(end).FigureId()];
            if (figure.Type() != M3Figure.eType.NORMAL)
            {
                mCurrentGiftType = figure.Type();
                mCurrentGiftPosition = begin;

                if (figure.Type() == M3Figure.eType.RAINBOW)
                    mRainbowColor = mIds[mBoard.GetCellByPos(begin).FigureId()].Color();
            }
            else
            {
                mStepBeginPos = begin;
                mStepEndPos = end;
            }

            mMode = eMode.INACTION;
            MoveObjects(begin, end);
        }
        
        public void GameProcess()
        {
            if (mMode == eMode.INACTION || mMode == eMode.COMPLETENESS)
                return;
  
            if (mMode == eMode.INACTION_BY_TIMER)
            {
                if (mTimer > 0)
                {
                    --mTimer;
                }
                else
                {
                    mTimer = TIMER_VALUE;
                    mMode = eMode.PERFORMING_COMBOS;
                }
            }
            else if (mMode == eMode.CHECK_STATE)
            {
                if (mCurrentGiftPosition.IsExist())
                {
                    switch (mCurrentGiftType)
                    {
                        case M3Figure.eType.BOMB:
                            UseBomb(mCurrentGiftPosition);
                            break;
                        case M3Figure.eType.RAINBOW:
                            UseRainbow(mCurrentGiftPosition, mRainbowColor);
                            break;
                        case M3Figure.eType.HORIZONTAL:
                            UseHorizontal(mCurrentGiftPosition);
                            break;
                        case M3Figure.eType.VERTICAL:
                            UseVertical(mCurrentGiftPosition);
                            break;
                    }
                    
                    mCurrentGiftType = M3Figure.eType.NONE;
                    mCurrentGiftPosition.Reset();
                    return;
                }
                
                var combos = new List<M3Combo>();
                if (CheckCombos(ref combos))
                {
                    mCurrentCombos = combos;

                    if (mStepBeginPos.IsExist() || mStepEndPos.IsExist())
                    {
                        mStepBeginPos.Reset();
                        mStepEndPos.Reset();
                    }

                    mMode = eMode.PERFORMING_COMBOS;
                }
                else
                {
                    mMode = eMode.INACTION;

                    if (mStepBeginPos.IsExist() || mStepEndPos.IsExist())
                    {
                        MoveObjects(mStepEndPos, mStepBeginPos);
                        mNeedLogStep = false;
                        mStepBeginPos.Reset();
                        mStepEndPos.Reset();
                        return;
                    }
                    
                    if (mNeedLogStep)
                    {
                        M3Settings.DebugLog("M3: Step is completed");
                        M3Settings.OnStepComplete();
                    }
                    else
                    {
                        mNeedLogStep = true;
                    }

                    if (CheckExit())
                    {
                        mMode = eMode.COMPLETENESS;
                        M3Settings.DebugLog("M3: Level is passed");
                        M3Settings.OnGameComplete();
                    }

                    if (!IsPossibleCombosExist())
                    {
                        M3Settings.DebugLog("M3: Need to shuffle");

                        FindVoids(ref mVoidsForReshuffle);
                        DestroyAllObjects();
                    }
                }
            }
            else if (mMode == eMode.PERFORMING_COMBOS)
            {
                if (mVoidsForReshuffle.Count > 0)
                {
                    ShuffleFigures(mVoidsForReshuffle);
                    mVoidsForReshuffle.Clear();
                    return;
                }
                
                if (mCurrentCombos.Count > 0)
                {
                    mMode = eMode.INACTION;
                    UseCombos(mCurrentCombos);
                    mCurrentCombos.Clear();
                    mMode = eMode.INACTION_BY_TIMER;
                    return;
                }

                var voids = new List<M3Position>();
                if (IsVoidsExist(ref voids))
                {
                    mMode = eMode.INACTION;
                    CreateNewFigures(voids);
                    return;
                }

                var level = LowerLevelSearch();
                if (level.Count > 0)
                {
                    mMode = eMode.INACTION;
                    LowerLevelDrop(level);
                }
                else
                {
                    mMode = eMode.CHECK_STATE;
                }
            }
        }

        private bool IsPossibleCombosExist()
        {
            var combos = new List<M3Combo>();    
            if (CheckCombos(ref combos))
                return true;
            
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    if (y == 0)
                    {
                        if (x == mBoard.Width() - 1)
                            continue;
                        
                        if (SwapAndCheck(x, y, false))
                            return true;
                    }
                    else
                    {
                        if (x == mBoard.Width() - 1)
                        {
                            if (SwapAndCheck(x, y, true))
                                return true;
                        }
                        else
                        {
                            if (SwapAndCheck(x, y, false))
                                return true;
                            
                            if (SwapAndCheck(x, y, true))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool SwapAndCheck(int x, int y, bool down)
        {
            var pos1 = new M3Position(x, y);
            var pos2 = down ? new M3Position(x, y - 1) : new M3Position(x + 1, y);

            var cell1 = mBoard.GetCellByPos(pos1);
            var cell2 = mBoard.GetCellByPos(pos2);
            
            if (cell1.IsEmpty() || cell1.IsChainState() || cell2.IsEmpty() || cell2.IsChainState())
                return false;

            mBoard.SwapCells(pos1, pos2);
            var combos = new List<M3Combo>();
            var isExist = CheckCombos(ref combos);
            mBoard.SwapCells(pos2, pos1);
                        
            return isExist;
        }

        private void UseBomb(M3Position pos)
        {
            const int border = 3;
            
            var beginX = pos.X() - 1;
            var beginY = pos.Y() - 1;

            var endX = beginX + border;
            var endY = beginY + border;

            if (beginX < 0)
            {
                beginX = 0;
                endX = border - 1;
            }
            else if (endX > mBoard.Width())
            {
                endX = mBoard.Width();
                beginX = endX - (border - 1);
            }
            
            if (beginY < 0)
            {
                beginY = 0;
                endY = border - 1;
            }
            else if (endY > mBoard.Height())
            {
                endY = mBoard.Height();
                beginY = endY - (border - 1);
            }

            for (var y = beginY; y < endY; ++y)
            {
                for (var x = beginX; x < endX; ++x)
                {
                    var current = new M3Position(x, y);
                    DestroyObjectGeneral(current);
                }
            }
        }

        private void UseRainbow(M3Position pos, M3Figure.eColor color)
        {
            if (color == M3Figure.eColor.NONE)
                return;
            
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var position = new M3Position(x, y);
                    if (color == mIds[mBoard.GetCellByPos(position).FigureId()].Color())
                    {
                        DestroyObjectGeneral(position);
                    }
                }
            }
            
            DestroyObjectGeneral(pos);
        }

        private void UseHorizontal(M3Position pos)
        {
            for (var x = 0; x < mBoard.Width(); ++x)
            {
                var current = new M3Position(x, pos.Y());
                DestroyObjectGeneral(current);
            }
        }

        private void UseVertical(M3Position pos)
        {
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                var current = new M3Position(pos.X(), y);
                DestroyObjectGeneral(current);
            }
        }

        private bool IsVoidsExist(ref List<M3Position> positions)
        {
            positions.Clear();

            var isVoidsExist = false;
            var upperLevelY = mBoard.Height() - 1;

            for (var x = 0; x < mBoard.Width(); ++x)
            {
                var pos = new M3Position(x, upperLevelY);
                if (mBoard.IsCellEmpty(pos))
                {
                    positions.Add(pos);
                    isVoidsExist = true;
                }
            }
            
            return isVoidsExist;
        }

        private void CreateNewFigures(List<M3Position> positions)
        {
            var rand = new Random();

            foreach (var pos in positions)
            {
                var color = (M3Figure.eColor) rand.Next(1, 6);
                CreateObject(M3Figure.eType.NORMAL, color, pos);
            }
        }

        public void UseCombos(List<M3Combo> combos)
        {
            foreach (var combo in combos)
            {
                var positions = combo.Positions();

                foreach (var pos in positions)
                {
                    DestroyObjectGeneral(pos);
                }

                var giftType = combo.GiftType();
                if (giftType != M3Figure.eType.NONE)
                {
                    var giftPos = combo.GiftPosition();
                    CreateObject(giftType, M3Figure.eColor.NONE, giftPos);
                }
            }
        }

        private void DestroyObjectGeneral(M3Position pos)
        {
            if (mBoard.GetCellByPos(pos).IsFrameState())
            {
                DestroyFrameObject(pos, mBoard.GetCellByPos(pos).State());
                DestroyObject(pos);
            }
            else if (mBoard.GetCellByPos(pos).IsChainState())
            {
                DestroyFrameObject(pos, mBoard.GetCellByPos(pos).State());
            }
            else
            {
                DestroyObject(pos);
            }
        }

        public bool CheckCombos(ref List<M3Combo> combos)
        {
            combos.Clear();
            
            List<M3Combo> vertPossibleCombos = VerticalBypass();
            List<M3Combo> horPossibleCombos = HorizontalBypass();
            combos = AnalizeCombos(vertPossibleCombos, horPossibleCombos);

            return (combos.Count != 0);
        }
      
        private List<M3Combo> HorizontalBypass()
        {
            var combos = new List<M3Combo>();

            for (int y = 0; y < mBoard.Height(); ++y)
            {
                for (int x = 0; x < mBoard.Width();)
                {
                    int firstFigId = mBoard.GetCellByPos(new M3Position(x, y)).FigureId();
                    int secondFigId = mBoard.GetCellByPos(new M3Position(x+1, y)).FigureId();

                    if (firstFigId == 0 || secondFigId == 0)
                    {
                        ++x;
                        continue;
                    }
                    
                    M3Figure.eColor firstFigColor = mIds[firstFigId].Color();
                    M3Figure.eColor secondFigColor = mIds[secondFigId].Color();

                    if (firstFigColor != secondFigColor)
                    {
                        ++x;
                        continue;
                    }
                    
                    int thirdFigId = mBoard.GetCellByPos(new M3Position(x+2, y)).FigureId();
                    
                    if (0 == thirdFigId)
                    {
                        x += 2;
                        continue;
                    }
                    
                    M3Figure.eColor thirdFigColor = mIds[thirdFigId].Color();

                    if (firstFigColor != thirdFigColor)
                    {
                        x += 2;
                        continue;
                    }
                    
                    x = CreatePossibleComboHor(x, y, ref combos);  
                }
            }
            
            return combos;
        }
        
        private int CreatePossibleComboHor(int x, int y, ref List<M3Combo> combos)
        {
            int size = 3;
            var positions = new List<M3Position>();
            
            int firstFigId = mBoard.GetCellByPos(new M3Position(x, y)).FigureId();
            int forthFigId = mBoard.GetCellByPos(new M3Position(x+3, y)).FigureId();
            int fifthFigId = mBoard.GetCellByPos(new M3Position(x+4, y)).FigureId();
            
            M3Figure.eColor firstFigColor = mIds[firstFigId].Color();

            M3Figure.eColor forthFigColor = M3Figure.eColor.NONE;
            M3Figure.eColor fifthFigColor = M3Figure.eColor.NONE;
            
            if (forthFigId != 0)
                forthFigColor = mIds[forthFigId].Color();
            
            if (fifthFigId != 0)
                fifthFigColor = mIds[fifthFigId].Color();

            if (firstFigColor == forthFigColor)
            {
                size = 4;

                if (firstFigColor == fifthFigColor)
                {
                    size = 5;
                }
            }

            for (int k = 0; k < size; ++k)
            {
                positions.Add(new M3Position(x+k, y));
            }
            
            combos.Add(new M3Combo(M3Combo.eType.NONE, firstFigColor, positions));

            return x + size;
        }
        
        private List<M3Combo> VerticalBypass()
        {
            var combos = new List<M3Combo>();

            for (int x = 0; x < mBoard.Width(); ++x)
            {
                for (int y = 0; y < mBoard.Height();)
                {
                    int firstFigId = mBoard.GetCellByPos(new M3Position(x, y)).FigureId();
                    int secondFigId = mBoard.GetCellByPos(new M3Position(x, y+1)).FigureId();

                    if (firstFigId == 0 || secondFigId == 0)
                    {
                        ++y;
                        continue;
                    }
                    
                    M3Figure.eColor firstFigColor = mIds[firstFigId].Color();
                    M3Figure.eColor secondFigColor = mIds[secondFigId].Color();

                    if (firstFigColor != secondFigColor)
                    {
                        ++y;
                        continue;
                    }
                    
                    int thirdFigId = mBoard.GetCellByPos(new M3Position(x, y+2)).FigureId();
                    
                    if (0 == thirdFigId)
                    {
                        y += 2;
                        continue;
                    }
                    
                    M3Figure.eColor thirdFigColor = mIds[thirdFigId].Color();

                    if (firstFigColor != thirdFigColor)
                    {
                        y += 2;
                        continue;
                    }
                    
                    y = CreatePossibleComboVert(x, y, ref combos);  
                }
            }
            
            return combos;
        }

        private int CreatePossibleComboVert(int x, int y, ref List<M3Combo> combos)
        {
            int size = 3;
            var positions = new List<M3Position>();
            
            int firstFigId = mBoard.GetCellByPos(new M3Position(x, y)).FigureId();
            int forthFigId = mBoard.GetCellByPos(new M3Position(x, y+3)).FigureId();
            int fifthFigId = mBoard.GetCellByPos(new M3Position(x, y+4)).FigureId();
            
            M3Figure.eColor firstFigColor = mIds[firstFigId].Color();

            M3Figure.eColor forthFigColor = M3Figure.eColor.NONE;
            M3Figure.eColor fifthFigColor = M3Figure.eColor.NONE;
            
            if (forthFigId != 0)
                forthFigColor = mIds[forthFigId].Color();
            
            if (fifthFigId != 0)
                fifthFigColor = mIds[fifthFigId].Color();

            if (firstFigColor == forthFigColor)
            {
                size = 4;

                if (firstFigColor == fifthFigColor)
                {
                    size = 5;
                }
            }

            for (int k = 0; k < size; ++k)
            {
                positions.Add(new M3Position(x, y+k));
            }
            
            combos.Add(new M3Combo(M3Combo.eType.NONE, firstFigColor, positions));

            return y + size;
        }

        private List<M3Combo> AnalizeCombos(List<M3Combo> combosVert, List<M3Combo> combosHor)
        {
            var combos = new List<M3Combo>();
            var numbers = new List<int>();

            foreach (var comboV in combosVert)
            {
                bool hasIntersection = false;
                int counter = 0;
                
                foreach (var comboH in combosHor)
                {
                    M3Position interPos = new M3Position();
                    hasIntersection = IsIntersection(comboV, comboH, ref interPos);

                    if (hasIntersection)
                    {
                        M3Combo combo = CreateCombo(comboV, comboH, interPos);
                        combos.Add(combo);
                        numbers.Add(counter);
                        break;
                    }

                    ++counter;
                }

                if (!hasIntersection)
                {
                    M3Combo combo = CreateCombo(comboV);
                    combos.Add(combo);
                }
            }
            
            numbers.Sort();

            for (int i = 0; i < combosHor.Count; ++i)
            {
                int index = numbers.BinarySearch(i);
                if (index < 0)
                {
                    M3Combo combo = CreateCombo(combosHor[i]);
                    combos.Add(combo);
                }
            }

            return combos;
        }

        private bool IsIntersection(M3Combo combo1, M3Combo combo2, ref M3Position interPos)
        {
            interPos.Reset();
            bool isIntersection = false;

            foreach (var pos1 in combo1.Positions())
            {
                if (isIntersection)
                    break;
                
                foreach (var pos2 in combo2.Positions())
                {
                    if (pos1.IsEqual(pos2))
                    {
                        isIntersection = true;
                        interPos.SetX(pos1.X());
                        interPos.SetY(pos1.Y());
                        break;
                    }
                }
            }

            return isIntersection;
        }

        private M3Combo CreateCombo(M3Combo comboVert, M3Combo comboHor, M3Position interPos)
        {
            var type = M3Combo.eType.NONE;
            var color = M3Figure.eColor.NONE;
            var positions = new List<M3Position>();
            
            color = comboVert.Color();

            foreach (var pos in comboVert.Positions())
            {
                if (!pos.IsEqual(interPos))
                    positions.Add(pos);
            }
            
            foreach (var pos in comboHor.Positions())
            {
                if (!pos.IsEqual(interPos))
                    positions.Add(pos);
            }
            
            positions.Add(interPos);

            if (positions.Count == 7)
            {
                type = M3Combo.eType.TAVR7;
            }
            else
            {
                int maxY = comboVert.Positions()[0].Y();
                int minY = comboVert.Positions()[0].Y();
                
                foreach (var pos in comboVert.Positions())
                {
                    if (maxY < pos.Y())
                        maxY = pos.Y();
                    
                    if (minY > pos.Y())
                        minY = pos.Y();
                }
                
                int maxX = comboHor.Positions()[0].X();
                int minX = comboHor.Positions()[0].X();
                
                foreach (var pos in comboHor.Positions())
                {
                    if (maxX < pos.X())
                        maxX = pos.X();
                    
                    if (minX > pos.X())
                        minX = pos.X();
                }

                bool condVert = interPos.Y() == maxY || interPos.Y() == minY;
                bool condHor = interPos.X() == maxX || interPos.X() == minX;

                if (condVert && condHor)
                    type = M3Combo.eType.CORNER5;
                else
                    type = M3Combo.eType.TAVR5;
            }

            return new M3Combo(type, color, positions);
        }

        private M3Combo CreateCombo(M3Combo combo)
        {
            int size = combo.Positions().Count;
            var type = M3Combo.eType.NONE;

            if (3 == size)
                type = M3Combo.eType.LINE3;
            else if (4 == size)
                type = M3Combo.eType.LINE4;
            else if (5 == size)
                type = M3Combo.eType.LINE5;

            return new M3Combo(type, combo.Color(), combo.Positions());
        }

        private List<DropInfo> LowerLevelSearch()
        {
            var level = new List<DropInfo>();
            var rand = new Random();
            
            for (var y = 1; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var cell = mBoard.GetCellByPos(new M3Position(x, y));
                    if (cell.IsEmpty())
                        continue;
                    
                    var lowCell = mBoard.GetCellByPos(new M3Position(x, y - 1));
                    if (lowCell.IsEmpty())
                    {
                        if (cell.IsChainState())
                        {
                            var leftPos = new M3Position(x - 1, y);
                            var rightPos = new M3Position(x + 1, y);
                            
                            bool addLeftInfo = true;
                            
                            foreach (var dropInfo in level)
                            {
                                if (dropInfo.pos.IsEqual(leftPos))
                                {
                                    addLeftInfo = false;
                                    break;
                                }
                            }
                            
                            if (leftPos.IsExist() &&
                                !mBoard.GetCellByPos(leftPos).IsEmpty() &&
                                !mBoard.GetCellByPos(leftPos).IsChainState() &&
                                addLeftInfo)
                            {
                                level.Add(new DropInfo(leftPos, DropInfo.eDirection.LEFT));
                            }
                            else if (rightPos.IsExist() &&
                                     !mBoard.GetCellByPos(rightPos).IsEmpty() &&
                                     !mBoard.GetCellByPos(rightPos).IsChainState() &&
                                     !mBoard.GetCellByPos(new M3Position(x + 1, y - 1)).IsEmpty())
                            {
                                level.Add(new DropInfo(rightPos, DropInfo.eDirection.RIGHT));
                            }
                        }
                        else
                        {
                            level.Add(new DropInfo(new M3Position(x, y), DropInfo.eDirection.DOWN));
                        }
                    }
                }
            }

            return level;
        }

        private void LowerLevelDrop(List<DropInfo> level)
        {
            foreach (var drop in level)
            {
                var pos = drop.pos;
                var dir = drop.dir;
                var target = new M3Position(pos.X(), pos.Y());

                if (dir == DropInfo.eDirection.DOWN)
                {
                    var counter = 1;

                    while (true)
                    {
                        target.SetY(pos.Y() - counter);

                        if (target.Y() == 0)
                            break;

                        var cell = mBoard.GetCellByPos(new M3Position(target.X(), target.Y() - 1));
                        if (!cell.IsEmpty())
                            break;

                        ++counter;
                    }
                }
                else if (dir == DropInfo.eDirection.LEFT)
                {
                    target = new M3Position(pos.X() + 1, pos.Y() - 1);
                }
                else if (dir == DropInfo.eDirection.RIGHT)
                {
                    target = new M3Position(pos.X() - 1, pos.Y() - 1);
                }

                DropObject(pos, target);
            }
        }

        public bool CheckExit()
        {
            var isCompleted = true;
            
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var position = new M3Position(x, y);
                    if (mBoard.GetCellByPos(position).IsFrameState() || mBoard.GetCellByPos(position).IsChainState())
                    {
                        isCompleted = false;
                        break;
                    }
                }

                if (!isCompleted)
                    break;
            }

            return isCompleted;
        }
        
        public void Reset()
        {
            for (var y = 0; y < mBoard.Height(); ++y)
            {
                for (var x = 0; x < mBoard.Width(); ++x)
                {
                    var position = new M3Position(x, y);
                    DestroyObject(position);
                }
            }
            
            mBoard.CleanBoard();
            mIds.Clear();
        }
    }
}