using Photon.Pun;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private bool _isTaken;
    private PhotonView _photonView;
    
    public bool IsTaken => _isTaken;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SetTaken(bool value)
    {
        _photonView.RPC("SetTaken", RpcTarget.OthersBuffered, value);
        _isTaken = value;
    }
}
