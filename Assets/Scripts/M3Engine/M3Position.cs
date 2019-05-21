namespace Match3Engine
{
    public class M3Position
    {
        private int mCoordX;
        private int mCoordY;

        public M3Position()
        {
            mCoordX = -1;
            mCoordY = -1;
        }
        
        public M3Position(int pos)
        {
            mCoordX = pos;
            mCoordY = pos;
        }
        
        public M3Position(int x, int y)
        {
            mCoordX = x;
            mCoordY = y;
        }

        public void SetX(int x)
        {
            mCoordX = x;
        }
        
        public void SetY(int y)
        {
            mCoordY = y;
        }

        public int X() { return mCoordX; }
        public int Y() { return mCoordY; }

        public bool IsExist()
        {
            return (mCoordX != -1 && mCoordY != -1);
        }

        public bool IsEqual(M3Position other)
        {
            return (mCoordX == other.X()) && (mCoordY == other.Y());
        }

        public void Reset()
        {
            mCoordX = -1;
            mCoordY = -1;
        }
    }
}