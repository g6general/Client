using System;
using System.Diagnostics;

namespace Match3Engine
{
    public class M3Engine
    {
        private M3GamePlay mGamePlay;

        public M3Engine(int width, int height)
        {
            M3Settings.Init(width, height);
            mGamePlay = new M3GamePlay(width, height);
        }

        public void StartGame(int level)
        {
            mGamePlay.ShuffleFigures();
            mGamePlay.LoadFrames(level);

            M3Settings.DebugLog("M3: Game start");
        }
        
        public void RestartGame(int level)
        {
            ResetGame();
            StartGame(level);
        }
        
        public void ResetGame()
        {
            mGamePlay.Reset();

            M3Settings.DebugLog("M3: Game reset");
        }
        
        public void Move(M3Position begin, M3Position end)
        {
            mGamePlay.Move(begin, end);
        }

        public void StepCompleted()
        {
            mGamePlay.SetEngineMode(M3GamePlay.eMode.CHECK_STATE);
        }
        
        public void FallCompleted()
        {
            mGamePlay.SetEngineMode(M3GamePlay.eMode.INACTION_BY_TIMER);
        }
        
        public void CreationCompleted()
        {
            mGamePlay.SetEngineMode(M3GamePlay.eMode.INACTION_BY_TIMER);
        }
        
        public void DestructionCompleted()
        {
            mGamePlay.SetEngineMode(M3GamePlay.eMode.INACTION_BY_TIMER);
        }

        public void GameProcess()
        {
            mGamePlay.GameProcess();
        }

        public M3GamePlay.LevelInfo GetLevelInfo(int level)
        {
            var levels = mGamePlay.GetLevelsInfo();
            return levels[level];
        }

        public int GetNumberOfLevels()
        {
            var levels = mGamePlay.GetLevelsInfo();
            return levels.Count;
        }
        
        public void SetPrintFunction(M3Settings.PrintFunction func)
        {
            M3Settings.SetPrintDelegate(func);
        }

        public void SetCreateFunction(M3Settings.CreateFunction func)
        {
            M3Settings.SetCreateDelegate(func);
        }
        
        public void SetDestroyFunction(M3Settings.DestroyFunction func)
        {
            M3Settings.SetDestroyDelegate(func);
        }

        public void SetCreateFrameFunction(M3Settings.CreateFrameFunction func)
        {
            M3Settings.SetCreateFrameDelegate(func);
        }
        
        public void SetDestroyFrameFunction(M3Settings.DestroyFrameFunction func)
        {
            M3Settings.SetDestroyFrameDelegate(func);
        }

        public void SetGameCompleteFunction(M3Settings.OnGameCompleteFunction func)
        {
            M3Settings.SetGameCompleteDelegate(func);
        }
        
        public void SetStepCompleteFunction(M3Settings.OnStepCompleteFunction func)
        {
            M3Settings.SetStepCompleteDelegate(func);
        }
        
        public void SetActionFunction(M3Position pos, M3Settings.ActionFunction func)
        {
            M3Settings.SetActionDelegate(pos, func);
        }
    }
}