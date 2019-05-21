using System;

namespace Match3Engine
{
    public static class M3Settings
    {
        public enum eState { WAITING, MOVE, FALL };
        
        public delegate void PrintFunction(string text);
        public delegate void CreateFunction(M3Figure.eType type, M3Figure.eColor color, M3Position pos);
        public delegate void DestroyFunction(M3Position pos);
        public delegate void CreateFrameFunction(M3Cell.eState state, M3Position pos);
        public delegate void DestroyFrameFunction(M3Position pos, M3Cell.eState state);
        public delegate void ActionFunction(eState state, M3Position targetPos);
        public delegate void OnGameCompleteFunction();
        
        private static PrintFunction mPrintFunc;
        private static CreateFunction mCreateFunc;
        private static DestroyFunction mDestroyFunc;
        private static CreateFrameFunction mCreateFrameFunc;
        private static DestroyFrameFunction mDestroyFrameFunc;
        private static ActionFunction[,] mActionFuncs;
        private static OnGameCompleteFunction mGameCompleteFunc;

        public static void SetPrintDelegate(PrintFunction func) { mPrintFunc = func; }
        
        public static void SetCreateDelegate(CreateFunction func) { mCreateFunc = func; }

        public static void SetDestroyDelegate(DestroyFunction func) { mDestroyFunc = func; }
        
        public static void SetCreateFrameDelegate(CreateFrameFunction func) { mCreateFrameFunc = func; }
        
        public static void SetDestroyFrameDelegate(DestroyFrameFunction func) { mDestroyFrameFunc = func; }
        
        public static void SetGameCompleteDelegate(OnGameCompleteFunction func) { mGameCompleteFunc = func; }

        public static void SetActionDelegate(M3Position pos, ActionFunction func)
        {
            mActionFuncs[pos.X(), pos.Y()] = func;
        }

        public static ActionFunction GetActionDelegate(M3Position pos)
        {
            return mActionFuncs[pos.X(), pos.Y()];
        }

        public static void Init(int width, int height)
        {
            mPrintFunc = Console.WriteLine;
            mActionFuncs = new ActionFunction[width, height];
        }
        
        public static void DebugLog(string text) { mPrintFunc(text); }
        
        public static void CreateObject(M3Figure.eType type, M3Figure.eColor color, M3Position pos) { mCreateFunc(type, color, pos); }
        
        public static void DestroyObject(M3Position pos) { mDestroyFunc(pos); }

        public static void CreateFrameObject(M3Cell.eState state, M3Position pos) { mCreateFrameFunc(state, pos); }
        
        public static void DestroyFrameObject(M3Position pos, M3Cell.eState state) { mDestroyFrameFunc(pos, state); }
        
        public static void OnGameComplete() { mGameCompleteFunc(); }

        public static void SetAction(eState state, M3Position currentPos, M3Position targetPos)
        {
            mActionFuncs[currentPos.X(), currentPos.Y()](state, targetPos);
        }
    }
}