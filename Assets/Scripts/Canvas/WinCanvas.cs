using Photon.Pun;
using UnityEngine;

public class WinCanvas : MonoBehaviour
{
    private PhotonView _photonView;
    
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameEventManager.Instance.onTaskComplete += ShowText;
            GameEventManager.Instance.onStartNewGame += HideText;
        }
        _photonView = GetComponent<PhotonView>();
        HideText();
    }
    
    private void ShowText()
    {
        _photonView.RPC("ChangeActiveRPC" , RpcTarget.All, true);
    }
    
    private void HideText()
    {
        _photonView.RPC("ChangeActiveRPC" , RpcTarget.All, false);
    }
    
    [PunRPC]
    private void ChangeActiveRPC(bool value)
    {
        GameEventManager.Instance.TriggerOnChangeCanMove(!value);
        gameObject.SetActive(value);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.onTaskComplete -= ShowText;
        GameEventManager.Instance.onStartNewGame -= HideText;
    }
}
