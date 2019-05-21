namespace Match3Engine
{
    public class M3Figure
    {
        public enum eType { NONE, NORMAL, HORIZONTAL, VERTICAL, BOMB, RAINBOW };
        public enum eColor { NONE, GREEN, PINK, RED, VIOLET, YELLOW };

        private eType mType;
        private eColor mColor;

        public M3Figure()
        {
            mType = eType.NONE;
            mColor = eColor.NONE;
        }

        public M3Figure(eType type, eColor color)
        {
            mType = type;
            mColor = color;
        }

        public eType Type() { return mType; }
        public eColor Color() { return mColor; }
    }
}