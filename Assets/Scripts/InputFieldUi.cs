using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldUi : MonoBehaviour
{
    public void OnNickChanged()
    {
        var newNick = GameObject.Find("input_field_nick").GetComponent<InputField>().text;
        GameObject.Find("Main Camera").GetComponent<GameData>().SetNickname(newNick);
    }
}
