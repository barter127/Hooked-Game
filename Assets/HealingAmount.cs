using UnityEngine;

public class HealingAmount : MonoBehaviour
{
    
    [SerializeField]
    private int m_healValue;

    public int HealValue
    {
        get { return m_healValue; }
        private set
        {
            m_healValue = value;
        }
    }
}
