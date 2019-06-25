using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Match3Engine;
using UnityEditor;
using UnityEngine.UI;
using Random = System.Random;

public class Board : MonoBehaviour
{
    public int mWidth;
    public int mHeight;

    public int mCurrentLevel;
    public int mNumberOfLevels;
    public int mStepsCounter;
    public int mLevelsCounter;

    public GameObject[] mPrefabs;
    public GameObject[,] mFigures;
    public List<Frame> mFrames;
    
    public Camera mCamera;

    public M3Position mFirstTouchPos;

    public M3Engine mEngine;
    
    public GameObject mBuyStepsMenu;

    private bool mGameCompleted;

    public float mFigurePosZ = -5f;
    private float mFramePosZ = -2f;
    
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
        
        GameObject.Find("settings_menu").SetActive(false);
        mBuyStepsMenu.SetActive(false);

        Init();
        StartMatch3();

        var magicNumber = 7.39f;
        mCamera.orthographicSize = (magicNumber * Screen.height) / (2 * Screen.width);
    }

    private void StartMatch3()
    {
        mEngine.StartGame(0);
        mNumberOfLevels = mEngine.GetNumberOfLevels();
        
        var rand = new Random();
        mCurrentLevel = rand.Next(0, mNumberOfLevels);

        mEngine.RestartGame(mCurrentLevel);
        var info = mEngine.GetLevelInfo(mCurrentLevel);
        mStepsCounter = info.steps;
        mLevelsCounter = 1;

        GameObject.Find("steps_counter").GetComponent<Text>().text = mStepsCounter.ToString();
        GameObject.Find("level_counter").GetComponent<Text>().text = mLevelsCounter.ToString();
    }

    void Update()
    {
        mEngine.GameProcess();
    }

    private void Init()
    {
        mWidth = 7;
        mHeight = 7;

        mCurrentLevel = 0;
        mNumberOfLevels = 0;
        mStepsCounter = 0;
        mLevelsCounter = 0;

        mFigures = new GameObject[mWidth, mHeight];
        mFrames = new List<Frame>();
        mGameCompleted = false;

        mEngine = new M3Engine(mWidth, mHeight);
        mEngine.SetPrintFunction(Debug.Log);
        mEngine.SetCreateFunction(CreateObject);
        mEngine.SetDestroyFunction(DestroyObject);
        mEngine.SetCreateFrameFunction(CreateFrameObject);
        mEngine.SetDestroyFrameFunction(DestroyFrameObject);
        mEngine.SetGameCompleteFunction(OnGameCompleted);
        mEngine.SetStepCompleteFunction(OnStepCompleted);
        
        mFirstTouchPos = new M3Position();
    }

    private void OnGameCompleted()
    {
        var info1 = mEngine.GetLevelInfo(mCurrentLevel);
        
        var profileCoins = GameObject.Find("Main Camera").GetComponent<ProfileManager>().mProfile.GetCoins();
        GameObject.Find("Main Camera").GetComponent<ProfileManager>().mProfile.SetCoins(profileCoins + info1.coins);

        var profileRecord = GameObject.Find("Main Camera").GetComponent<ProfileManager>().mProfile.GetRecord();
        if (mLevelsCounter > profileRecord)
            GameObject.Find("Main Camera").GetComponent<ProfileManager>().mProfile.SetRecord(mLevelsCounter);
        
        ++mCurrentLevel;
        ++mLevelsCounter;
        
        if (mCurrentLevel >= mNumberOfLevels)
            mCurrentLevel = 0;
        
        var info2 = mEngine.GetLevelInfo(mCurrentLevel);
        mStepsCounter = info2.steps;
        
        GameObject.Find("level_counter").GetComponent<Text>().text = mLevelsCounter.ToString();
        GameObject.Find("steps_counter").GetComponent<Text>().text = mStepsCounter.ToString();

        mEngine.RestartGame(mCurrentLevel);
    }

    private void OnStepCompleted()
    {
        --mStepsCounter;
        GameObject.Find("steps_counter").GetComponent<Text>().text = mStepsCounter.ToString();

        if (mStepsCounter <= 0)
        {
            Debug.Log("Game over!");
            mBuyStepsMenu.SetActive(true);

            var buttonText1 = GameObject.Find("button_buy_1").GetComponentInChildren<Text>();
            var buttonText2 = GameObject.Find("button_buy_2").GetComponentInChildren<Text>();
            var buttonText3 = GameObject.Find("button_buy_3").GetComponentInChildren<Text>();
            
            var gameData = GameObject.Find("Main Camera").GetComponent<GameData>();
            buttonText1.text = gameData.GetString("ingame_purchase_button_1");
            buttonText2.text = gameData.GetString("ingame_purchase_button_2");
            buttonText3.text = gameData.GetString("ingame_purchase_button_3");
        }
    }

    public void OnCloseBsm()
    {
        mBuyStepsMenu.SetActive(false);
        
        mCurrentLevel = 0;
        mLevelsCounter = 1;
        var info = mEngine.GetLevelInfo(mCurrentLevel);
        mStepsCounter = info.steps;

        GameObject.Find("steps_counter").GetComponent<Text>().text = mStepsCounter.ToString();
        GameObject.Find("level_counter").GetComponent<Text>().text = mLevelsCounter.ToString();

        mEngine.RestartGame(mCurrentLevel);
    }

    private void BuyStepsBsm(int steps, int price)
    {
        var currentCoins = GameObject.Find("Main Camera").GetComponent<ProfileManager>().mProfile.GetCoins();
        if (currentCoins < price)
            return;
        
        mBuyStepsMenu.SetActive(false);
 
        mStepsCounter += steps;
        GameObject.Find("steps_counter").GetComponent<Text>().text = mStepsCounter.ToString();

        currentCoins -= price;
        GameObject.Find("Main Camera").GetComponent<ProfileManager>().mProfile.SetCoins(currentCoins); 
    }
    
    public void OnBuy1Bsm()
    {
        BuyStepsBsm(5, 100);
    }

    public void OnBuy2Bsm()
    {
        BuyStepsBsm(10, 200);
    }
    
    public void OnBuy3Bsm()
    {
        BuyStepsBsm(15, 300);
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
        Vector3 position = new Vector3((float) pos.X(), (float) pos.Y(), mFigurePosZ);
        
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

        Vector3 position = new Vector3(pos.X(), pos.Y(), mFramePosZ);

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
