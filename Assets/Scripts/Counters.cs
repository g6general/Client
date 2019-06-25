using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counters : MonoBehaviour
{
    public void SetCounterLabels()
    {
        var gameData = GameObject.Find("Main Camera").GetComponent<GameData>();

        GameObject.Find("steps_text").GetComponent<Text>().text = gameData.GetString("steps_counter");
        GameObject.Find("level_text").GetComponent<Text>().text = gameData.GetString("level_counter");
        GameObject.Find("record_text").GetComponent<Text>().text = gameData.GetString("record_counter");

        var textTransform = GameObject.Find("level_text").GetComponent<Text>().rectTransform;
        textTransform.sizeDelta = new Vector2(270f, textTransform.sizeDelta.y);
    }
}
