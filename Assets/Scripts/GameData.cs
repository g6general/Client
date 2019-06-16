using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    private string mGameVersion;
    private Settings mSettings;
    private Dictionary<string, string> mTextsRu;
    private Dictionary<string, string> mTextsEn;

    void Awake()
    {
        SetCurrentGameVersion();
    }
    
    void Start()
    {
        mTextsRu = new Dictionary<string, string>();
        mTextsEn = new Dictionary<string, string>();
        mSettings = new Settings();
        
        LoadSettings();
        LoadTexts();
    }

    private void SetCurrentGameVersion()
    {
        mGameVersion = "1.0.0";
    }
    
    public string GetCurrentGameVersion()
    {
        return mGameVersion;
    }
    
    void OnApplicationQuit()
    {
        UnloadSettings();
    }
    
    private string GetConfigData(string dirPath, string fileName)
    {
        var configPath = Path.Combine(dirPath, fileName);
        string data;

        if (File.Exists(configPath))
        {
#if UNITY_EDITOR || UNITY_IOS
            data = File.ReadAllText(configPath);
#elif UNITY_ANDROID
            WWW reader = new WWW (configPath);
            while (!reader.isDone) {}
            data = reader.text;
#endif
            return data;
        }

        return "";
    }

    private void LoadSettings()
    {
        var dirPath = Application.streamingAssetsPath + "/GeneratedConfigs/";
        var configData = GetConfigData(dirPath,"settings.json");

        if (configData.Length > 0)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var serializer = new DataContractJsonSerializer(typeof(Settings));

            writer.Write(configData);
            writer.Flush();
            stream.Position = 0;

            mSettings = (Settings) serializer.ReadObject(stream) ?? new Settings();
        }
        else
        {
            mSettings = new Settings();
        }
    }

    private void UnloadSettings()
    {
        var stream = new MemoryStream();
        var serializer = new DataContractJsonSerializer(typeof(Settings));
        
        serializer.WriteObject(stream, mSettings);
        
        var data = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var dirPath = Application.streamingAssetsPath + "/GeneratedConfigs/";
        var configPath = Path.Combine(dirPath, "settings.json");

        File.WriteAllText(configPath, data);
    }
    
    private void LoadTexts()
    {
        var dirPath = Application.streamingAssetsPath + "/";
        var configData = GetConfigData(dirPath, "texts_en.xml");

        var texts = XElement.Parse(configData);
        var text = texts.FirstNode;

        while (text != null)
        {
            var textNode = (XElement) text;
            var textKey = textNode.Name.ToString();
            var textValue = textNode.FirstAttribute.Value;

            mTextsEn.Add(textKey, textValue);
            
            text = text.NextNode;
        }
        
        configData = GetConfigData(dirPath, "texts_ru.xml");

        texts = XElement.Parse(configData);
        text = texts.FirstNode;
        
        while (text != null)
        {
            var textNode = (XElement) text;
            var textKey = textNode.Name.ToString();
            var textValue = textNode.FirstAttribute.Value;

            mTextsRu.Add(textKey, textValue);
            
            text = text.NextNode;
        }
    }

    public string GetString(string key)
    {
        var texts = mTextsEn;

        if (mSettings.GetLanguage() == Settings.eLanguage.RUSSIAN)
            texts = mTextsRu;

        return texts[key];
    }

    public float GetMusicVolume() { return mSettings.GetMusicVolume(); }
    public float GetSoundVolume() { return mSettings.GetSoundVolume(); }
    public Settings.eLanguage GetLanguage() { return mSettings.GetLanguage(); }

    public void SetMusicVolume(float volume) { mSettings.SetMusicVolume(volume); }
    public void SetSoundVolume(float volume) { mSettings.SetSoundVolume(volume); }
    public void SetLanguage(Settings.eLanguage language) { mSettings.SetLanguage(language); }
}

[DataContract]
public class Settings
{
    public enum eLanguage { RUSSIAN, ENGLISH };

    [DataMember] private string Language = "en";
    [DataMember] private string MusicVolume = "0.5";
    [DataMember] private string SoundVolume = "0.5";

    public eLanguage GetLanguage()
    {
        var lang = eLanguage.ENGLISH;
        
        if (Language == "ru")
            return eLanguage.RUSSIAN;

        return lang;
    }

    public float GetMusicVolume()
    {
        return float.Parse(MusicVolume, CultureInfo.InvariantCulture.NumberFormat);
    }
    
    public float GetSoundVolume()
    {
        return float.Parse(SoundVolume, CultureInfo.InvariantCulture.NumberFormat);
    }

    public void SetLanguage(eLanguage language)
    {
        Language = (language == eLanguage.RUSSIAN) ? "ru" : "en";
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume.ToString();
    }
    
    public void SetSoundVolume(float volume)
    {
        SoundVolume = volume.ToString();
    }
}
