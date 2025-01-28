using UnityEngine;
using System.Diagnostics;

public class SceneTimer : MonoBehaviour
{
    private Stopwatch stopwatch;

    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Just for testing purposes, you can change the condition as needed
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log("Time spent in scene: " + stopwatch.ElapsedMilliseconds + " ms");
        }
    }
}
