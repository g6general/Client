using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    public GameObject mMusicOn;
    public GameObject mMusicOff;
    public GameObject mSoundOn;
    public GameObject mSoundOff;
    
    void Start()
    {
        mMusicOn.SetActive(false);
        mMusicOff.SetActive(true);
        mSoundOn.SetActive(false);
        mSoundOff.SetActive(true);
    }

    public void OnMusicValueChange(float slider)
    {
        if (slider == 0)
        {
            mMusicOn.SetActive(false);
            mMusicOff.SetActive(true);
        }
        else
        {
            mMusicOn.SetActive(true);
            mMusicOff.SetActive(false);
        }
    }
    
    public void OnSoundValueChange(float slider)
    {
        if (slider == 0)
        {
            mSoundOn.SetActive(false);
            mSoundOff.SetActive(true);
        }
        else
        {
            mSoundOn.SetActive(true);
            mSoundOff.SetActive(false);
        }
    }
}
