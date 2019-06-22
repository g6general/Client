using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource mMusic;
    public AudioSource mSound;

    void Start()
    {
        mMusic.loop = true;
        mMusic.Play();
    }
    
    public void SetMusicVolume(float volume, bool loading = false)
    {
        mMusic.volume = volume;
        
        if (!loading)
            GameObject.Find("Main Camera").GetComponent<GameData>().SetMusicVolume(volume);
    }
    
    public void SetSoundVolume(float volume, bool loading = false)
    {
        mSound.volume = volume;
        
        if (!loading)
            GameObject.Find("Main Camera").GetComponent<GameData>().SetSoundVolume(volume);
    }

    public void PlaySound()
    {
        mSound.Play();
    }
}
