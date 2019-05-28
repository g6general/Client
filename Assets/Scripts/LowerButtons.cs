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

    void Start()
    {
        GameObject.Find("canvas_top").GetComponent<Canvas>().enabled = false;
        GameObject.Find("canvas_shop").GetComponent<Canvas>().enabled = false;
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
            float buttonDelta = Screen.width * 0.2f;

            var trans = mButtons[n].GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(trans.sizeDelta.x + buttonDelta, trans.sizeDelta.y);
            
            trans = mButtons[mButtonBeforePressed].GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(trans.sizeDelta.x - buttonDelta, trans.sizeDelta.y);
            
            var trans1st = mButtons[0].GetComponent<RectTransform>();
            var trans2nd = mButtons[1].GetComponent<RectTransform>();
            var trans3rd = mButtons[2].GetComponent<RectTransform>();

            if (n == 0)
            {
                trans1st.position = new Vector3(trans1st.position.x + buttonDelta / 2, trans1st.position.y, trans1st.position.y);
                
                if (mButtonBeforePressed == 1)
                {
                    trans2nd.position = new Vector3(trans2nd.position.x + buttonDelta / 2, trans2nd.position.y, trans2nd.position.z);
                }
                else if (mButtonBeforePressed == 2)
                {
                    trans2nd.position = new Vector3(trans2nd.position.x + buttonDelta, trans2nd.position.y, trans2nd.position.z);
                    trans3rd.position = new Vector3(trans3rd.position.x + buttonDelta / 2, trans3rd.position.y, trans3rd.position.z);
                }
            }
            else if (n == 1)
            {
                if (mButtonBeforePressed == 0)
                {
                    trans1st.position = new Vector3(trans1st.position.x - buttonDelta / 2, trans1st.position.y, trans1st.position.y);
                    trans2nd.position = new Vector3(trans2nd.position.x - buttonDelta / 2, trans2nd.position.y, trans2nd.position.y);
                }
                else if (mButtonBeforePressed == 2)
                {
                    trans2nd.position = new Vector3(trans2nd.position.x + buttonDelta / 2, trans2nd.position.y, trans2nd.position.z);
                    trans3rd.position = new Vector3(trans3rd.position.x + buttonDelta / 2, trans3rd.position.y, trans3rd.position.z);
                }
            }
            else if (n == 2)
            {
                trans3rd.position = new Vector3(trans3rd.position.x - buttonDelta / 2, trans3rd.position.y, trans3rd.position.z);
                
                if (mButtonBeforePressed == 0)
                {
                    trans1st.position = new Vector3(trans1st.position.x - buttonDelta / 2, trans1st.position.y, trans1st.position.y);
                    trans2nd.position = new Vector3(trans2nd.position.x - buttonDelta, trans2nd.position.y, trans2nd.position.y);
                }
                else if (mButtonBeforePressed == 1)
                {
                    trans2nd.position = new Vector3(trans2nd.position.x - buttonDelta / 2, trans2nd.position.y, trans2nd.position.z);
                }
            }
        }
        
        mButtonBeforePressed = n;
    }
}
