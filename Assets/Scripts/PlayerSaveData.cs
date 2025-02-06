// Holds player statistics and settings data
// Will be converted to Json.
using System;

[Serializable]
public class PlayerSaveData
{
    // Statistics
    public int m_totalDeaths;
    public int m_totalKills;
    public float m_totalInGameSeconds;
}
