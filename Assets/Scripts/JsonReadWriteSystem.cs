using UnityEngine;
using System.IO;

public class JsonReadWriteSystem : MonoBehaviour
{
    #region Player Statistics
    public static void SaveStatisticData(int totalDeaths, int totalKills, float totalInGameSeconds)
    {
        PlayerSaveData data = new PlayerSaveData();
        data.m_totalDeaths = totalDeaths;
        data.m_totalKills = totalKills;
        data.m_totalInGameSeconds = totalInGameSeconds;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/PlayerSaveDataFile.json", json);
    }

    public static PlayerSaveData LoadStatisticData()
    {
        if (!File.Exists(Application.dataPath + "/PlayerSaveDataFile.json"))
        {
            return null;
        }

        string json = File.ReadAllText(Application.dataPath + "/PlayerSaveDataFile.json");
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
        return data;
    }

    #endregion

    #region Settings
    public static void SaveSettingsData(SettingsData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/SettingsDataFile.json", json);
    }

    public static SettingsData LoadSettingsData()
    {
        if (!File.Exists(Application.dataPath + "/SettingsDataFile.json"))
        {
            return null;
        }

        string json = File.ReadAllText(Application.dataPath + "/SettingsDataFile.json");
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);
        return data;
    }

    public static void SetMusicVolume(float volume)
    {
        SettingsData data = LoadSettingsData();
        data.musicVolume = volume;
        SaveSettingsData(data);
    }

    public static void SetSFXVolume(float volume)
    {
        SettingsData data = LoadSettingsData();
        data.sfxVolume = volume;
        SaveSettingsData(data);
    }


    public static float GetMusicVolume()
    {
        SettingsData data = LoadSettingsData();

        return data.musicVolume;
    }

    public static float GetSFXVolume()
    {
        SettingsData data = LoadSettingsData();

        return data.sfxVolume;
    }
    #endregion
}
