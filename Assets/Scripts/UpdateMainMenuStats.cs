using UnityEngine;
using TMPro;

public class UpdateMainMenuStats : MonoBehaviour
{
    /// <summary>
    /// Used to update statistics panel text in main menu.
    /// </summary>

    [SerializeField] TextMeshProUGUI m_statText;

    void OnEnable()
    {
        PlayerSaveData data = JsonReadWriteSystem.LoadStatisticData();

        // File doesn't exist (probably because first time playing)
        if (data == null)
        {
            // Make new Json with default vals.
            JsonReadWriteSystem.SaveStatisticData(0, 0, 0);

            // Load new Json.
            data = JsonReadWriteSystem.LoadStatisticData();
        }

        m_statText.text = "Total Deaths: " + data.m_totalDeaths + "\n \n" +
                          "Total Kills: " + data.m_totalKills + "\n \n" +
                          "Play Time:" + FloatToTimer(data.m_totalInGameSeconds);
    }

    string FloatToTimer(float unformatTimer)
    {
        int minutes = Mathf.FloorToInt(unformatTimer / 60);
        int seconds = Mathf.FloorToInt(unformatTimer % 60);

        string formatString = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formatString;
    }
}
