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

    private void LoadProfile()
    {
        var configData = GetConfigData("profile.json");
        
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        var serializer = new DataContractJsonSerializer(typeof(Profile));
        
        writer.Write(configData);
        writer.Flush();
        stream.Position = 0;
        
        mProfile = (Profile)serializer.ReadObject(stream) ?? new Profile();
    }
    
    private void UnloadProfile()
    {
        var stream = new MemoryStream();
        var serializer = new DataContractJsonSerializer(typeof(Profile));
        
        serializer.WriteObject(stream, mProfile);
        
        var data = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var configPath = Path.Combine(Application.streamingAssetsPath + "/", "profile.json");

        File.WriteAllText(configPath, data);
    }
}

[DataContract] public class Profile
{
    [DataMember] private string mNickname = "nickname";
    [DataMember] private string mCoins = "0";
    [DataMember] private string mRecord = "0";
    
    string GetNickname() { return mNickname; }

    int GetCoins()
    {
        return int.Parse(mCoins, CultureInfo.InvariantCulture.NumberFormat);
    }

    int GetRecord()
    {
        return int.Parse(mRecord, CultureInfo.InvariantCulture.NumberFormat);
    }
    
    void SetNickname(string nickname) { mNickname = nickname; }

    void SetCoins(int coins)
    {
        mCoins = coins.ToString();
    }

    void SetRecord(int record)
    {
        mRecord = record.ToString();
    }
}
