using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerButtons : MonoBehaviour
{
    public GameObject[] mButtons;
    private int mButtonBeforePressed = 1;
    
    private Coroutine mCameraMoving;

    public float[] mXCameraPositions;
    private float mCameraEpsilon = 0.1f;
    private float mCameraVelocityCoef = 0.3f;

    private List<float> mTopButtonPositions;
    private List<float> mGameButtonPositions;
    private List<float> mShopButtonPositions;

    void Start()
    {
        GameObject.Find("canvas_top").GetComponent<Canvas>().enabled = false;
        GameObject.Find("canvas_shop").GetComponent<Canvas>().enabled = false;

        var activeButton = 365f;
        var inactiveButton = 217f;
        
        mTopButtonPositions = new List<float>
        {
            activeButton / 2,
            inactiveButton / 2
        };

        mGameButtonPositions = new List<float>
        {
            inactiveButton + activeButton / 2,
            activeButton + inactiveButton / 2,
            inactiveButton + inactiveButton / 2
        };
        
        mShopButtonPositions = new List<float>
        {
            activeButton + inactiveButton + inactiveButton / 2,
            2 * inactiveButton + activeButton / 2
        };
    }

    public void OnButtonPress(int n)
    {
        ListTopInfo(n == 0);
        ListShopInfo(n == 2);

        if (mCameraMoving != null)
            StopCoroutine(mCameraMoving);

        mCameraMoving = StartCoroutine(CameraMoving(n));

        ChangeButtonsView(n);
    }

    private void ListTopInfo(bool mode)
    {
        if (mode)
            GameObject.Find("canvas_top").GetComponent<Canvas>().enabled = true;
        else
            GameObject.Find("canvas_top").GetComponent<Canvas>().enabled = false;
    }
    
    private void ListShopInfo(bool mode)
    {
        if (mode)
            GameObject.Find("canvas_shop").GetComponent<Canvas>().enabled = true;
        else
            GameObject.Find("canvas_shop").GetComponent<Canvas>().enabled = false;
    }

    IEnumerator CameraMoving(int n)
    {
        while (Mathf.Abs(transform.position.x - mXCameraPositions[n]) > mCameraEpsilon)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(mXCameraPositions[n], transform.position.y, transform.position.z), mCameraVelocityCoef);
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(mXCameraPositions[n], transform.position.y, transform.position.z);
    }

    void ChangeButtonsView(int n)
    {
        if (n != mButtonBeforePressed)
        {
            var buttonDelta = mButtons[mButtonBeforePressed].GetComponent<RectTransform>().sizeDelta.x -
                              mButtons[n].GetComponent<RectTransform>().sizeDelta.x;

            var trans = mButtons[n].GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(trans.sizeDelta.x + buttonDelta, trans.sizeDelta.y);
            
            trans = mButtons[mButtonBeforePressed].GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(trans.sizeDelta.x - buttonDelta, trans.sizeDelta.y);
            
            var trans1st = mButtons[0].GetComponent<RectTransform>();
            var trans2nd = mButtons[1].GetComponent<RectTransform>();
            var trans3rd = mButtons[2].GetComponent<RectTransform>();

            if (n == 0)
            {
                trans1st.anchoredPosition = new Vector3(mTopButtonPositions[0], trans1st.anchoredPosition.y);
                trans2nd.anchoredPosition = new Vector3(mGameButtonPositions[1], trans2nd.anchoredPosition.y);
                
                if (mButtonBeforePressed == 2)
                {
                    trans3rd.anchoredPosition = new Vector3(mShopButtonPositions[0], trans3rd.anchoredPosition.y);
                }
            }
            else if (n == 1)
            {
                trans2nd.anchoredPosition = new Vector3(mGameButtonPositions[0], trans2nd.anchoredPosition.y);
                
                if (mButtonBeforePressed == 0)
                {
                    trans1st.anchoredPosition = new Vector3(mTopButtonPositions[1], trans1st.anchoredPosition.y);
                }
                else if (mButtonBeforePressed == 2)
                {
                    trans3rd.anchoredPosition = new Vector3(mShopButtonPositions[0], trans3rd.anchoredPosition.y);
                }
            }
            else if (n == 2)
            {
                trans3rd.anchoredPosition = new Vector3(mShopButtonPositions[1], trans3rd.anchoredPosition.y);
                trans2nd.anchoredPosition = new Vector3(mGameButtonPositions[2], trans2nd.anchoredPosition.y);
                
                if (mButtonBeforePressed == 0)
                {
                    trans1st.anchoredPosition = new Vector3(mTopButtonPositions[1], trans1st.anchoredPosition.y);
                }
            }
        }
        
        mButtonBeforePressed = n;
    }
}
