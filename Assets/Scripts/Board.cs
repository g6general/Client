using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Match3Engine;

public class Board : MonoBehaviour
{
    public int mWidth;
    public int mHeight;

    public int mCurrentLevel;
    public int mNumberOfLevels;

    public GameObject[] mPrefabs;
    public GameObject[,] mFigures;
    public List<Frame> mFrames;
    
    public Camera mCamera;

    public M3Position mFirstTouchPos;

    public M3Engine mEngine;

    private bool mGameCompleted;
    
    public struct Frame
    {
        public M3Position position;
        public M3Cell.eState state;
        public GameObject figure;

        public Frame(M3Position position, M3Cell.eState state, GameObject figure)
        {
            this.position = position;
            this.state = state;
            this.figure = figure;
        }
    }
    
    void Start()
    {
        Debug.Log($"Screen width: {Screen.width}, Screen height: {Screen.height}");
        
        Init();
        mEngine.StartGame(mCurrentLevel);
    }

    void Update()
    {
        mEngine.GameProcess();
/*
#if !UNITY_EDITOR
        if (Screen.orientation == ScreenOrientation.Portrait)
            mCamera.orthographicSize = 7f;
        else if (Screen.orientation == ScreenOrientation.Landscape)
            mCamera.orthographicSize = 3.7f;
#endif
*/
    }

    private void Init()
    {
        mWidth = 7;
        mHeight = 7;
        mCurrentLevel = 1;
        mNumberOfLevels = 5;
        mFigures = new GameObject[mWidth, mHeight];
        mFrames = new List<Frame>();
        /*
        Vector3 position = new Vector3(3.0f, 3.0f, -1f);
        mCamera.transform.SetPositionAndRotation(position, Quaternion.identity);
        
        #if UNITY_EDITOR
            mCamera.orthographicSize = 3.7f;
        #endif
        */
        mGameCompleted = false;

        mEngine = new M3Engine(mWidth, mHeight);
        mEngine.SetPrintFunction(Debug.Log);
        mEngine.SetCreateFunction(CreateObject);
        mEngine.SetDestroyFunction(DestroyObject);
        mEngine.SetCreateFrameFunction(CreateFrameObject);
        mEngine.SetDestroyFrameFunction(DestroyFrameObject);
        mEngine.SetGameCompleteFunction(OnGameCompleted);
        
        mFirstTouchPos = new M3Position();
    }

    private void OnGameCompleted()
    {
        ++mCurrentLevel;

        if (mCurrentLevel <= mNumberOfLevels)
        {
            mEngine.RestartGame(mCurrentLevel);
        }
        else
        {
            mCamera.backgroundColor = Color.yellow;
            mGameCompleted = true;
        }
    }

    public bool IsGameCompleted() { return mGameCompleted; }

    private void CreateObject(M3Figure.eType type, M3Figure.eColor color, M3Position pos)
    {
        int index = -1;
        
        if (M3Figure.eType.NORMAL == type)
        {
            switch (color)
            {
                case M3Figure.eColor.GREEN:
                    index = 0;
                    break;
                case M3Figure.eColor.PINK:
                    index = 1;
                    break;
                case M3Figure.eColor.RED:
                    index = 2;
                    break;
                case M3Figure.eColor.VIOLET:
                    index = 3;
                    break;
                case M3Figure.eColor.YELLOW:
                    index = 4;
                    break; 
            }
        }
        else if (M3Figure.eType.HORIZONTAL == type)
        {
            index = 5; 
        }
        else if (M3Figure.eType.VERTICAL == type)
        {
            index = 6; 
        }
        else if (M3Figure.eType.BOMB == type)
        {
            index = 7; 
        }
        else if (M3Figure.eType.RAINBOW == type)
        {
            index = 8;
        }
        
        GameObject prefab = mPrefabs[index];
        Vector2 position = new Vector2((float) pos.X(), (float) pos.Y());
        
        GameObject figure = Instantiate(prefab, position, Quaternion.identity);
        figure.name = "Figure";
        figure.GetComponent<Dot>().StepCompleted += StepCompleted;
        figure.GetComponent<Dot>().FallCompleted += FallCompleted;
        mFigures[pos.X(), pos.Y()] = figure;
        
        mEngine.SetActionFunction(new M3Position(pos.X(), pos.Y()), figure.GetComponent<Dot>().SetAction);
        mEngine.CreationCompleted();
    }

    private void DestroyObject(M3Position pos)
    {
        Destroy(mFigures[pos.X(), pos.Y()]);
        
        mEngine.SetActionFunction(new M3Position(pos.X(), pos.Y()), null);
        mEngine.DestructionCompleted();
    }

    private void CreateFrameObject(M3Cell.eState state, M3Position pos)
    {
        if (state == M3Cell.eState.NONE || state == M3Cell.eState.FREE)
            return;

        Vector2 position = new Vector2(pos.X(), pos.Y());
        
        int index = 10;
        GameObject prefab = mPrefabs[index];
        GameObject figure = Instantiate(prefab, position, Quaternion.identity);
        figure.name = "Frame";
        mFrames.Add(new Frame(pos, M3Cell.eState.FRAME, figure));
        
        if (state == M3Cell.eState.CHAIN)
        {
            int index1 = 9;
            GameObject prefab1 = mPrefabs[index1];
            GameObject figure1 = Instantiate(prefab1, position, Quaternion.identity);
            figure1.name = "Chain";
            mFrames.Add(new Frame(pos, state, figure1));
        }
    }

    private void DestroyFrameObject(M3Position pos, M3Cell.eState state)
    {
        var index = -1;

        for (var i = 0; i < mFrames.Count; ++i)
        {
            if (mFrames[i].position.IsEqual(pos) && mFrames[i].state == state)
            {
                index = i;
                break;
            }
        }
        
        if (index != -1)
        {
            Destroy(mFrames[index].figure);
            mFrames.Remove(mFrames[index]);
            mEngine.DestructionCompleted();
        }
    }

    public void StepCompleted()
    {
        mEngine.StepCompleted();
    }
    
    public void FallCompleted()
    {
        mEngine.FallCompleted();
    }

    public void ResetFirstTouch()
    {
        mFirstTouchPos.SetX(-1);
        mFirstTouchPos.SetY(-1);
    }

    public bool IsSecondTouch()
    {
        return (mFirstTouchPos.X() != -1 && mFirstTouchPos.Y() != -1);
    }
}
