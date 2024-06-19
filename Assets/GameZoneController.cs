using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZoneController : MonoBehaviour
{
    private void Start()
    {
        GameEventManager.Instance.TriggerOnStartNewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameEventManager.Instance.TriggerOnStartNewGame();
        }
    }

}
