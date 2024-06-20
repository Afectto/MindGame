using Photon.Pun;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private bool _isTaken;
    private PhotonView _photonView;
    
    public bool IsTaken => _isTaken;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SetTaken(bool value)
    {
        _isTaken = value;
    }
    
    public void SetTakenRPC(bool value)
    {
        _photonView.RPC("SetTaken", RpcTarget.OthersBuffered, value);
    }
}
