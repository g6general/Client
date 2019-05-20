using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Open()
    {
        mMenu.gameObject.SetActive(true);
    }

    private void Close()
    {
        mMenu.gameObject.SetActive(false);
    }
}
