using System;
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
    public void TriggerOnGrabCube(Vector3 pos)
    {
        onGrabCube?.Invoke(pos);
    }
    
    public event Action<Vector3> onPlaceCube;
    public void TriggerOnPlaceCube(Vector3 pos)
    {
        onPlaceCube?.Invoke(pos);
    }

    public event Action onStartNewGame;
    public  void TriggerOnStartNewGame()
    {
        onStartNewGame?.Invoke();
    }
    
    public event Action onTaskComplete;
    public  void TriggerOnTaskComplete()
    {
        onTaskComplete?.Invoke();
    }
    
    public event Action onJoinInRoom;
    public  void TriggerOnJoinInRoom()
    {
        onJoinInRoom?.Invoke();
    }

    public event Action<bool> OnChangeCanMove; 
    public  void TriggerOnChangeCanMove(bool value)
    {
        OnChangeCanMove?.Invoke(value);
    }
}
