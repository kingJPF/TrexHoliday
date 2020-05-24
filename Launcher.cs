using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("success");

        PhotonNetwork.JoinOrCreateRoom("Room",new Photon.Realtime.RoomOptions(){MaxPlayers = 3},default);

    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("00000000000");
            PhotonNetwork.Instantiate("Player1", new Vector3(8, 3, 0), Quaternion.identity, 0);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("111111111111");
            PhotonNetwork.Instantiate("Player2", new Vector3(8, 1, 0), Quaternion.identity, 0);
        }

    }
}
