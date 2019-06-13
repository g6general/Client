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

        GameObject.Find("slider_music").GetComponent<Slider>().value =
            GameObject.Find("Main Camera").GetComponent<GameData>().GetMusicVolume();
        
        GameObject.Find("slider_sound").GetComponent<Slider>().value =
            GameObject.Find("Main Camera").GetComponent<GameData>().GetSoundVolume();

        var lang = GameObject.Find("Main Camera").GetComponent<GameData>().GetLanguage();
        var langNumber = 0;

        if (lang == Settings.eLanguage.RUSSIAN)
            langNumber = 1;

        GameObject.Find("dropdown_lang").GetComponent<Dropdown>().value = langNumber;
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

        GameObject.Find("Main Camera").GetComponent<GameData>().SetMusicVolume(slider);
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
        
        GameObject.Find("Main Camera").GetComponent<GameData>().SetSoundVolume(slider);
    }

    public void OnLanguageChange()
    {
        int lang = GameObject.Find("dropdown_lang").GetComponent<Dropdown>().value;
        
        if (lang == 1)
            GameObject.Find("Main Camera").GetComponent<GameData>().SetLanguage(Settings.eLanguage.RUSSIAN);
        else
            GameObject.Find("Main Camera").GetComponent<GameData>().SetLanguage(Settings.eLanguage.ENGLISH);
        
        GameObject.Find("Main Camera").GetComponent<LowerButtons>().SetButtonLabels();
        GameObject.Find("button_close").GetComponent<SettingsMenu>().SetSettingsLabels();
        GameObject.Find("canvas_counters").GetComponent<Counters>().SetCounterLabels();
        GameObject.Find("Main Camera").GetComponent<GameData>().ChangeNickLanguage();
    }
}
