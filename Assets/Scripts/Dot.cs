using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Engine;

public class Dot : MonoBehaviour
{
    private Board mBoard;

    private float mEpsilon;
    private float mMoveVelocity;
    private float mFallVelocity;

    private M3Position mTargetPos;
    private M3Settings.eState mState;

    public delegate void Handler();
    public event Handler StepCompleted;
    public event Handler FallCompleted;

    void Start()
    {   
        mBoard = FindObjectOfType<Board>();
        mEpsilon = 0.05f;
        mMoveVelocity = 0.2f;
        mFallVelocity = 0.3f;

        mState = M3Settings.eState.WAITING;
        mTargetPos = new M3Position();
    }

    void Update()
    {
        if (!mTargetPos.IsExist() || M3Settings.eState.WAITING == mState)
            return;

        if (M3Settings.eState.MOVE == mState)
        {
            Vector3 targetPos = new Vector3(mTargetPos.X(), mTargetPos.Y(), mBoard.mFigurePosZ);

            if (transform.position.x == mTargetPos.X()) //vertical
            {
                if (Mathf.Abs(mTargetPos.Y() - transform.position.y) > mEpsilon)
                {
                    //move towards the target
                    transform.position = Vector3.Lerp(transform.position, targetPos, mMoveVelocity);
                }
                else
                {
                    //directly set position
                    transform.position = targetPos;
                    mBoard.mFigures[mTargetPos.X(), mTargetPos.Y()] = gameObject;

                    ResetTargetPosition();
                    mState = M3Settings.eState.WAITING;

                    if (StepCompleted != null)
                        StepCompleted();
                }
            }
            else if (transform.position.y == mTargetPos.Y()) //horizontal
            {
                if (Mathf.Abs(mTargetPos.X() - transform.position.x) > mEpsilon)
                {
                    //move towards the target
                    transform.position = Vector3.Lerp(transform.position, targetPos, mMoveVelocity);
                }
                else
                {
                    //directly set position 
                    transform.position = targetPos;
                    mBoard.mFigures[mTargetPos.X(), mTargetPos.Y()] = gameObject;

                    ResetTargetPosition();
                    mState = M3Settings.eState.WAITING;
                    
                    if (StepCompleted != null)
                        StepCompleted();
                }
            }
        }
        else if (M3Settings.eState.FALL == mState)
        {
            Vector3 targetPos = new Vector3(mTargetPos.X(), mTargetPos.Y(), mBoard.mFigurePosZ);
            
            if (Mathf.Abs(mTargetPos.Y() - transform.position.y) > mEpsilon)
            {
                //move towards the target
                transform.position = Vector3.Lerp(transform.position, targetPos, mFallVelocity);
            }
            else
            {
                //directly set position
                transform.position = targetPos;
                mBoard.mFigures[mTargetPos.X(), mTargetPos.Y()] = gameObject;

                ResetTargetPosition();
                mState = M3Settings.eState.WAITING;
                
                if (FallCompleted != null)
                    FallCompleted();
            }            
        }
    }

    public void SetAction(M3Settings.eState state, M3Position targetPos)
    {
        mState = state;
        mTargetPos.SetX(targetPos.X());
        mTargetPos.SetY(targetPos.Y());
    }
    
    private void SetTargetPosition(int x, int y)
    {
        mTargetPos.SetX(x);
        mTargetPos.SetX(y);
    }
    
    private void ResetTargetPosition()
    {
        SetTargetPosition(-1, -1);
    }
   
    private void OnMouseUp()
    {
        if (!mBoard.IsGameCompleted())
        {
            var currentX = (int) transform.position.x;
            var currentY = (int) transform.position.y;

            if (mBoard.IsSecondTouch())
            {
                M3Position firstPos = new M3Position(currentX, currentY);
                M3Position secondPos = new M3Position(mBoard.mFirstTouchPos.X(), mBoard.mFirstTouchPos.Y());

                mBoard.mEngine.Move(firstPos, secondPos);
                mBoard.ResetFirstTouch();

                GameObject.Find("Main Camera").GetComponent<Sounds>().PlaySound();
            }
            else
            {
                mBoard.mFirstTouchPos.SetX(currentX);
                mBoard.mFirstTouchPos.SetY(currentY);
            }
        }
    }
}
