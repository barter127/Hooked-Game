using UnityEngine;
using TMPro;

public class GameOverStatManager : MonoBehaviour
{
    // THiS WILL NEED TO BE SAVED TO A JSON. I LOVE JSON <3
    int m_deaths;
    float m_gameTime;
    int m_kills;

    [Header ("Text References")]
    [SerializeField] TextMeshProUGUI m_deathsText;
    [SerializeField] TextMeshProUGUI m_gameTimeText;
    [SerializeField] TextMeshProUGUI m_killsText;

    void OnEnable()
    {
        // Update strings.
        m_deathsText.text = "Hi";
        m_gameTimeText.text = FloatToTimer(m_gameTime);
        m_killsText.text = "Hi";
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
