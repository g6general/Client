using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mMenu;
    public void OnCall()
    {
        if (mMenu.gameObject.activeSelf == true)
            Close();
        else
            Open();
    }

    public void SetSettingsLabels()
    {
        var gameData = GameObject.Find("Main Camera").GetComponent<GameData>();

        GameObject.Find("text_music").GetComponent<Text>().text = gameData.GetString("music_label");
        GameObject.Find("text_sound").GetComponent<Text>().text = gameData.GetString("sound_label");
        GameObject.Find("text_language").GetComponent<Text>().text = gameData.GetString("language_label");
    }

    private void Open()
    {
        mMenu.gameObject.SetActive(true);
    }

    private void Close()
    {
        GameObject.Find("Main Camera").GetComponent<GameData>().SetNicknameIfEmpty();
        mMenu.gameObject.SetActive(false);
    }
}
