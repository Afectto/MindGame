using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : MonoBehaviour
{
    [SerializeField] private Text winText;
    [SerializeField] private Text restartText;
    
    void Start()
    {
        GameEventManager.Instance.onTaskComplete += ShowText;
        GameEventManager.Instance.onStartNewGame += HideText;
        HideText();
    }

    private void ShowText()
    {
        gameObject.SetActive(true);
    }

    private void HideText()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.onTaskComplete -= ShowText;
        GameEventManager.Instance.onStartNewGame -= HideText;
    }
}
