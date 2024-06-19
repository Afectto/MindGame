using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public event Action<Vector3> onGrabCube;
    public void TriggerGrabCube(Vector3 pos)
    {
        onGrabCube?.Invoke(pos);
    }
    
    public event Action<Vector3> onPlaceCube;
    public void TriggerPlaceCube(Vector3 pos)
    {
        onPlaceCube?.Invoke(pos);
    }

    public event Action onStartNewGame;
    public virtual void TriggerOnStartNewGame()
    {
        onStartNewGame?.Invoke();
    }
    
    public event Action onTaskComplete;
    public virtual void TriggerOnTaskComplete()
    {
        onTaskComplete?.Invoke();
    }
}
