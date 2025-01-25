using UnityEngine;
using System.IO;

public class JsonReadWriteSystem : MonoBehaviour
{
    public static void SaveDeathStatisticData(int totalDeaths, int totalKills)
    {
        PlayerSaveData data = new PlayerSaveData();
        data.m_totalDeaths = totalDeaths;
        data.m_totalKills = totalKills;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/PlayerSaveDataFile.json", json);
    }

    public static PlayerSaveData LoadDeathStatisticData()
    {
        string json = File.ReadAllText(Application.dataPath + "/PlayerSaveDataFile.json");
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
        return data;
    }
}
