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
    private Settings mSettings;
    private Dictionary<string, string> mTextsRu;
    private Dictionary<string, string> mTextsEn;

    private string mNickname;
    private bool IsPlaceholder;

    void Start()
    {
        mTextsRu = new Dictionary<string, string>();
        mTextsEn = new Dictionary<string, string>();
        mSettings = new Settings();
        
        LoadSettings();
        LoadTexts();
        LoadNickName();
    }

    private void LoadNickName()
    {
        IsPlaceholder = true;
        mNickname = GetString("nicknamePlaceholder");

        SetNicknameField();
        SetAllLabels();
    }

    public void SetNickname(string newNick)
    {
        IsPlaceholder = false;
        mNickname = newNick;
        SetAllLabels();
    }

    public void SetNicknameIfEmpty()
    {
        if (IsPlaceholder)
            return;

        if (mNickname.Length == 0)
        {
            SetNickname("Player");
            SetNicknameField();
        }
    }

    public void ChangeNickLanguage()
    {
        if (!IsPlaceholder)
            return;
        
        mNickname = GetString("nicknamePlaceholder");
        SetNicknameField();
        SetAllLabels();
    }

    private void SetAllLabels()
    {
        GameObject.Find("nickname_text").GetComponent<Text>().text = mNickname;
        GameObject.Find("top_nick_text_you").GetComponent<Text>().text = mNickname;
    }
    
    private void SetNicknameField()
    {
        if (IsPlaceholder)
        {
            var placeholder = GameObject.Find("input_field_nick").GetComponent<InputField>().placeholder;
            placeholder.GetComponent<Text>().text = mNickname;
        }
        else
        {
            GameObject.Find("input_field_nick").GetComponent<InputField>().text = mNickname;
        }
    }

    public string GetNickname()
    {
        return mNickname;
    }

    void OnApplicationQuit()
    {
        UnloadSettings();
    }
    
    private string GetConfigData(string fileName)
    {
        var configPath = Path.Combine(Application.streamingAssetsPath + "/", fileName);
        string data;
            
#if UNITY_EDITOR || UNITY_IOS
        data = File.ReadAllText(configPath);
#elif UNITY_ANDROID
        WWW reader = new WWW (configPath);
        while (!reader.isDone) {}
        data = reader.text;
#endif
        return data;
    }

    private void LoadSettings()
    {
        var configData = GetConfigData("settings.json");
        
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        var serializer = new DataContractJsonSerializer(typeof(Settings));
        
        writer.Write(configData);
        writer.Flush();
        stream.Position = 0;
        
        mSettings = (Settings)serializer.ReadObject(stream);
        
        if (mSettings == null)
            mSettings = new Settings();
    }

    private void UnloadSettings()
    {
        var stream = new MemoryStream();
        var serializer = new DataContractJsonSerializer(typeof(Settings));
        
        serializer.WriteObject(stream, mSettings);
        
        var data = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var configPath = Path.Combine(Application.streamingAssetsPath + "/", "settings.json");

        File.WriteAllText(configPath, data);
    }
    
    private void LoadTexts()
    {
        var configData = GetConfigData("texts_en.xml");

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
        
        configData = GetConfigData("texts_ru.xml");

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
