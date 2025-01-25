using UnityEngine;
using TMPro;

public class GameOverStatManager : MonoBehaviour
{
    /// <summary>
    /// On death, the panel attached will be enabled displaying in game values.
    /// Where appropriate, values are taken from the PlayerSaveData json.
    /// UI text are updated.
    /// </summary>
    int m_deaths;
    float m_gameTime;
    int m_kills;

    [Header ("Text References")]
    [SerializeField] TextMeshProUGUI m_deathsText;
    [SerializeField] TextMeshProUGUI m_gameTimeText;
    [SerializeField] TextMeshProUGUI m_killsText;

    void OnEnable()
    {
        // Get reference to saved statistics.
        PlayerSaveData data = JsonReadWriteSystem.LoadDeathStatisticDataFromJson();

        // Increment value and set text.
        m_deaths = data.m_totalDeaths;
        m_deaths++;
        m_deathsText.text = m_deaths.ToString();

        // Update kills value and set text.
        m_kills += data.m_totalKills;
        m_killsText.text = m_kills.ToString();

        m_gameTimeText.text = FloatToTimer(m_gameTime);
    }

    void Update()
    {
        m_gameTime += Time.deltaTime;
    }

    string FloatToTimer(float unformatTimer)
    {
        int minutes = Mathf.FloorToInt(unformatTimer / 60);
        int seconds = Mathf.FloorToInt(unformatTimer % 60);

        Debug.Log(minutes);
        Debug.Log(seconds);

        string formatString = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formatString;
    }
}
