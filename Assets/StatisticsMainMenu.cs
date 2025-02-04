using UnityEngine;

public class StatisticsMainMenu : MonoBehaviour
{
    void OnEnable()
    {
        JsonReadWriteSystem.LoadStatisticData();
    }
}
