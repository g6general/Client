namespace Match3Engine
{
    public class M3Cell
    {
        public enum eState { NONE, FREE, FRAME, CHAIN };

        private int mId;
        private eState mState;

        public M3Cell()
        {
            mId = 0;
            mState = eState.NONE;
        }
        
        public M3Cell(int id, eState state)
        {
            mId = id;
            mState = state;
        }

        public bool IsEmpty() { return (0 == mId); }

        public void Empty()
        {
            mId = 0;
        }

        public int FigureId() { return mId; }
        public void SetFigureId(int id) { mId = id; }
        
        public eState State() { return mState; }
        public void SetState(eState state) { mState = state; }

        public bool IsFreeState() { return (eState.FREE == mState); }
        public bool IsFrameState() { return (eState.FRAME == mState); }
        public bool IsChainState() { return (eState.CHAIN == mState); }

        public void SetFreeState() { mState = eState.FREE; }
        public void SetFrameState() { mState = eState.FRAME; }
        public void SetChainState() { mState = eState.CHAIN; }
    }
}