namespace Match3Engine
{
    public class M3Board
    {
        private int mWidth;
        private int mHeight;
        
        private M3Cell[,] mCells;

        public M3Board(int width, int height)
        {
            mWidth = width;
            mHeight = height;
            
            mCells = new M3Cell[width, height];
            
            for (var y = 0; y < mHeight; ++y)
            {
                for (var x = 0; x < mWidth; ++x)
                {
                    mCells[x, y] = new M3Cell();
                }
            }
        }

        public int Width() { return mWidth; }
        public int Height() { return mHeight; }

        public void FillCell(M3Position pos, int id = 0)
        {
            if (mCells[pos.X(), pos.Y()] == null)
            {
                mCells[pos.X(), pos.Y()] = new M3Cell(id, M3Cell.eState.FREE);
            }
            else
            {
                mCells[pos.X(), pos.Y()].SetFigureId(id);
            }
        }

        public void EmptyCell(M3Position pos)
        {
            if (mCells[pos.X(), pos.Y()] != null)
                mCells[pos.X(), pos.Y()].Empty();
        }

        public bool IsCellEmpty(M3Position pos)
        {
            return mCells[pos.X(), pos.Y()].IsEmpty();
        }

        public void SwapCells(M3Position pos1, M3Position pos2)
        {
            var firstId = mCells[pos1.X(), pos1.Y()].FigureId();
            var secondId = mCells[pos2.X(), pos2.Y()].FigureId();

            mCells[pos1.X(), pos1.Y()].SetFigureId(secondId);
            mCells[pos2.X(), pos2.Y()].SetFigureId(firstId);
        }

        public void CleanBoard()
        {
            for (int y = 0; y < mHeight; ++y)
            {
                for (int x = 0; x < mWidth; ++x)
                {
                    M3Position position = new M3Position(x, y);
                    EmptyCell(position);
                }
            }
        }

        public M3Cell GetCellById(int id)
        {
            bool isFound = false;
            M3Cell cell = null;
            
            for (int y = 0; y < mHeight; ++y)
            {
                if (!isFound)
                {
                    for (int x = 0; x < mWidth; ++x)
                    {
                        if (mCells[x, y].FigureId() == id)
                        {
                            cell = mCells[x, y];
                            isFound = true;
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return cell;
        }
        
        public M3Cell GetCellByPos(M3Position pos)
        {
            return !IsValidPosition(pos) ? new M3Cell() : mCells[pos.X(), pos.Y()];
        }

        private bool IsValidPosition(M3Position pos)
        {
            return ((pos.X() >= 0 && pos.X() < mHeight) && (pos.Y() >= 0 && pos.Y() < mWidth));
        }
    }
}