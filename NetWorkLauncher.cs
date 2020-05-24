using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetWorkLauncher : MonoBehaviourPunCallbacks
{
    public GameObject loginUI;
    public GameObject NameUI;
    public InputField RoomName;
    public InputField PlayerName;
    public GameObject roomListUI;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        NameUI.SetActive(true);
        Debug.Log("Join the Lobby");
        PhotonNetwork.JoinLobby();
    }

    public void PlayButton()
    {
        NameUI.SetActive(false);
        PhotonNetwork.NickName = PlayerName.text;
        loginUI.SetActive(true);
        if (PhotonNetwork.InLobby)
        {
            roomListUI.SetActive(true);
        }
    }

    public void JoinOrCreateButton()
    {
        if (RoomName.text.Length < 2)
            return;
        loginUI.SetActive(false);
        //RoomOptions option = new RoomOptions { MaxPlayers = 3 };
        PhotonNetwork.JoinOrCreateRoom(RoomName.text,new Photon.Realtime.RoomOptions(){ MaxPlayers = 3},default);
        // Debug.Log(RoomName.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(1);
    }
}
