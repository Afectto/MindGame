using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PlayerPrefab", Vector3.up, Quaternion.identity);
            PhotonNetwork.Instantiate("GameZone", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerPrefab", Vector3.up, Quaternion.identity);
        }
        GameEventManager.Instance.TriggerOnJoinInRoom();
    }
}