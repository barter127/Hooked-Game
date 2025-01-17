using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EnemyDamaged();
    public static event EnemyDamaged OnEnemyDamage;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
