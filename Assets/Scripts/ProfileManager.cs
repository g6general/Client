using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

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
}

[DataContract] public class Profile
{
    [DataMember] private string mNickname = "Player";
    [DataMember] private string mCoins = "0";
    [DataMember] private string mRecord = "0";
    
    public string GetNickname() { return mNickname; }

    public int GetCoins()
    {
        return int.Parse(mCoins, CultureInfo.InvariantCulture.NumberFormat);
    }

    public int GetRecord()
    {
        return int.Parse(mRecord, CultureInfo.InvariantCulture.NumberFormat);
    }
    
    public void SetNickname(string nickname) { mNickname = nickname; }

    public void SetCoins(int coins)
    {
        mCoins = coins.ToString();
    }

    public void SetRecord(int record)
    {
        mRecord = record.ToString();
    }
}
