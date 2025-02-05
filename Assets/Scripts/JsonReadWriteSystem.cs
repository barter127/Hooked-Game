using UnityEngine;
using System.IO;

public class JsonReadWriteSystem : MonoBehaviour
{
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
        string json = File.ReadAllText(Application.dataPath + "/PlayerSaveDataFile.json");
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
        return data;
    }
}
