using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
