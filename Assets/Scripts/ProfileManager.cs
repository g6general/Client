using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Profile mProfile { get; set; }

    void Awake()
    {
        LoadProfile();
    }

    void OnApplicationQuit()
    {
        UnloadProfile();
    }
    
    private string GetConfigData(string fileName)
    {
        var dirPath = Application.streamingAssetsPath + "/GeneratedConfigs/";
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

    private void LoadProfile()
    {
        var configData = GetConfigData("profile.json");

        if (configData.Length > 0)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var serializer = new DataContractJsonSerializer(typeof(Profile));

            writer.Write(configData);
            writer.Flush();
            stream.Position = 0;

            mProfile = (Profile) serializer.ReadObject(stream) ?? new Profile();
        }
        else
        {
            mProfile = new Profile();
        }

        CheckGameVersion();

        SetCoinsCounterUI();
        SetRecordCounterUI();
        SetNicknameUI();
    }

    private void CheckGameVersion()
    {
        var profileVersion = mProfile.GetProfileGameVersion();
        var currentVersion = GameObject.Find("Main Camera").GetComponent<GameData>().GetCurrentGameVersion();

        if (profileVersion == "0.0.0")
        {
            mProfile.SetProfileGameVersion(currentVersion);
            return;
        }

        var profileNumbers = profileVersion.Split('.');
        var currentNumbers = currentVersion.Split('.');
        
        if (profileNumbers[0] != currentNumbers[0])
        {
            Debug.Log("Major update detected.");
        }
        else if (profileNumbers[1] != currentNumbers[1])
        {
            Debug.Log("Minor update detected.");
        }
        else if (profileNumbers[2] != currentNumbers[2])
        {
            Debug.Log("Hotfix detected.");
        }
        
        mProfile.SetProfileGameVersion(currentVersion);
    }
    
    private void UnloadProfile()
    {
        var stream = new MemoryStream();
        var serializer = new DataContractJsonSerializer(typeof(Profile));
        
        serializer.WriteObject(stream, mProfile);
        
        var data = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var dirPath = Application.streamingAssetsPath + "/GeneratedConfigs/";
        var configPath = Path.Combine(dirPath, "profile.json");

        File.WriteAllText(configPath, data);
    }

    public void SetCoinsCounterUI()
    {
        GameObject.Find("coins_counter").GetComponent<Text>().text = mProfile.GetCoins().ToString();
        
        var rectTrans = GameObject.Find("coins_counter").GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(150, rectTrans.sizeDelta.y);
    }
    
    public void SetRecordCounterUI()
    {
        var record = mProfile.GetRecord().ToString();
        
        GameObject.Find("record_counter").GetComponent<Text>().text = record;
        var rectTrans1 = GameObject.Find("record_counter").GetComponent<RectTransform>();
        rectTrans1.sizeDelta = new Vector2(120, rectTrans1.sizeDelta.y);
        
        GameObject.Find("top_record_text_you").GetComponent<Text>().text = record;
        var rectTrans2 = GameObject.Find("top_record_text_you").GetComponent<RectTransform>();
        rectTrans2.sizeDelta = new Vector2(120, rectTrans2.sizeDelta.y);
    }

    public void SetNicknameUI()
    {
        var nickname = mProfile.GetNickname();
        GameObject.Find("nickname_text").GetComponent<Text>().text = nickname;
        GameObject.Find("top_nick_text_you").GetComponent<Text>().text = nickname;
        GameObject.Find("input_field_nick").GetComponent<InputField>().text = nickname;
    }
}

[DataContract] public class Profile
{
    [DataMember] private string mNickname = "Player";
    [DataMember] private string mCoins = "150";
    [DataMember] private string mRecord = "0";
    [DataMember] private string mGameVersion = "0.0.0";

    public string GetProfileGameVersion() { return mGameVersion; }
    
    public void SetProfileGameVersion(string newVersion) { mGameVersion = newVersion; }

    public string GetNickname() { return mNickname; }

    public int GetCoins()
    {
        return int.Parse(mCoins, CultureInfo.InvariantCulture.NumberFormat);
    }

    public int GetRecord()
    {
        return int.Parse(mRecord, CultureInfo.InvariantCulture.NumberFormat);
    }

    public void SetNickname(string nickname)
    {
        mNickname = nickname;
        GameObject.Find("Main Camera").GetComponent<ProfileManager>().SetNicknameUI();
    }

    public void SetCoins(int coins)
    {
        mCoins = coins.ToString();
        GameObject.Find("Main Camera").GetComponent<ProfileManager>().SetCoinsCounterUI();
    }

    public void SetRecord(int record)
    {
        mRecord = record.ToString();
        GameObject.Find("Main Camera").GetComponent<ProfileManager>().SetRecordCounterUI();
    }
}
