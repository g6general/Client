using System.Collections.Generic;

namespace Match3Engine
{
    public class M3Combo
    {
        public enum eType { NONE, LINE3, LINE4, LINE5, CORNER5, TAVR5, TAVR7 };

        private eType mType;
        private M3Figure.eColor mColor;
        private List<M3Position> mPositions;
        private M3Figure.eType mGiftType;
        private M3Position mGiftPosition;

        public M3Combo(eType type, M3Figure.eColor color, List<M3Position> positions)
        {
            mType = type;
            mColor = color;
            mPositions = positions;
            mGiftType = GetGiftType();
            mGiftPosition = GetGiftPosition();
        }

        private M3Figure.eType GetGiftType()
        {
            var giftType = M3Figure.eType.NONE;
             
            if (mType == eType.LINE5)
            {
                giftType = IsLineComboVertical() ? M3Figure.eType.VERTICAL : M3Figure.eType.HORIZONTAL;
            }
            else if (mType == eType.CORNER5 || mType == eType.TAVR5)
            {
                giftType = M3Figure.eType.BOMB;
            }
            else if (mType == eType.TAVR7)
            {
                giftType = M3Figure.eType.RAINBOW;
            }

            return giftType;
        }

        public bool IsLineComboVertical()
        {
            if (mType == eType.LINE3 || mType == eType.LINE4 || mType == eType.LINE5)
            {
                var first = mPositions[0];
                var second = mPositions[1];

                if (first.X() == second.X())
                    return true;
                else if (first.Y() == second.Y())
                    return false;
            }
            
            return false;
        }
        
        public bool IsLineComboHorizontal()
        {
            if (mType == eType.LINE3 || mType == eType.LINE4 || mType == eType.LINE5)
            {
                return !IsLineComboVertical();
            }
            
            return false;
        }

        private M3Position GetGiftPosition()
        {
            var giftPos = new M3Position();

            if (mType == eType.LINE5)
            {
                if (IsLineComboVertical())
                {
                    giftPos.SetX(mPositions[0].X());
                    
                    var coordsY = new List<int>();
                    foreach (var pos in mPositions)
                        coordsY.Add(pos.Y());
                    
                    coordsY.Sort();
                    
                    giftPos.SetY(coordsY[2]);
                }
                else
                {
                    giftPos.SetY(mPositions[0].Y());
                    
                    var coordsX = new List<int>();
                    foreach (var pos in mPositions)
                        coordsX.Add(pos.X());
                    
                    coordsX.Sort();
                    
                    giftPos.SetX(coordsX[2]);
                }
            }
            else if (mType == eType.CORNER5 || mType == eType.TAVR5 || mType == eType.TAVR7)
            {
                var found = false;

                foreach (var pos in mPositions)
                {
                    var sameX = false;
                    var sameY = false;

                    foreach (var subPos in mPositions)
                    {
                        if (pos.IsEqual(subPos))
                            continue;

                        if (pos.X() == subPos.X())
                            sameX = true;
                        
                        if (pos.Y() == subPos.Y())
                            sameY = true;

                        if (sameX && sameY)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    if (found)
                    {
                        giftPos = pos;
                        break;
                    }
                }
            }

            return giftPos;
        }

        public eType Type() { return mType; }
        public M3Figure.eColor Color() { return mColor; }
        public List<M3Position> Positions() { return mPositions; }

        public M3Figure.eType GiftType() { return mGiftType; }
        
        public M3Position GiftPosition() { return mGiftPosition; }
    }
}