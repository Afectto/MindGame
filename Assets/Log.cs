using System.Collections;
using UnityEngine;

public class Log : MonoBehaviour
{
    readonly uint _qsize = 20;  // number of messages to keep
    readonly Queue _myLogQueue = new Queue(); 
    
    void Start() 
    {
        Debug.Log("Started up logging.");
    }

    void OnEnable() 
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() 
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type) 
    {
        _myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            _myLogQueue.Enqueue(stackTrace);
        while (_myLogQueue.Count > _qsize)
            _myLogQueue.Dequeue();
    }

    void OnGUI() 
    {
        GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", _myLogQueue.ToArray()));
        GUILayout.EndArea();
    }
    
}
