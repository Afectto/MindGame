using Photon.Pun;
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
        PhotonNetwork.JoinOrCreateRoom("RoomName", new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Создание объекта персонажа на хосте
            GameObject player = PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
    }
}