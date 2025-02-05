using UnityEngine;
using TMPro;
using System.Diagnostics;

public class GameOverStatManager : MonoBehaviour
{
    /// <summary>
    /// On death, the panel attached will be enabled displaying in game values.
    /// Where appropriate, values are taken from the PlayerSaveData json.
    /// UI text are updated.
    /// </summary>

    Stopwatch m_stopwatch;

    static int m_deaths;
    static float m_totalInGameTime; // Time across all play sessions.
    static int m_kills;

    [Header ("Text References")]
    [SerializeField] TextMeshProUGUI m_deathsText;
    [SerializeField] TextMeshProUGUI m_gameTimeText;
    [SerializeField] TextMeshProUGUI m_killsText;

    // Initialise stopwatch. Disable GO as no code will be running.
    void Awake()
    { 
        m_stopwatch = new Stopwatch();
        m_stopwatch.Start();


        gameObject.SetActive (false);
    }

    void OnEnable()
    {
        // Get reference to saved statistics.
        PlayerSaveData data = JsonReadWriteSystem.LoadStatisticData();

        UpdateDeathText(data);
        UpdateKillsText();
        UpdateTimerText(data);

        JsonReadWriteSystem.SaveStatisticData(m_deaths, m_kills, m_totalInGameTime);
    }

    void UpdateDeathText(PlayerSaveData data)
    {
        // Increment value and set text.
        m_deaths = data.m_totalDeaths;
        m_deaths++;
        m_deathsText.text = m_deaths.ToString();
    }

    void UpdateKillsText()
    {
        // Set text.
        m_killsText.text = m_kills.ToString();
    }

    void UpdateTimerText(PlayerSaveData data)
    {
        m_stopwatch.Stop();
        m_gameTimeText.text = FloatToTimer((float) m_stopwatch.Elapsed.TotalSeconds);

        m_totalInGameTime = data.m_totalInGameSeconds;
        m_totalInGameTime += (float) m_stopwatch.Elapsed.TotalSeconds;
    }

    public static void IncrementKillCount()
    {
        GameOverStatManager.m_kills++;
    }

    string FloatToTimer(float unformatTimer)
    {
        int minutes = Mathf.FloorToInt(unformatTimer / 60);
        int seconds = Mathf.FloorToInt(unformatTimer % 60);

        string formatString = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formatString;
    }
}
