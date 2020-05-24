using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomLIstManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject roomPrefab;
    public Transform gridLayout;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        for(int i= 0; i < gridLayout.childCount; i++)
        {
            if(gridLayout.GetChild(i).gameObject.GetComponentInChildren<Text>().text == roomList[i].Name)
            {
                Destroy(gridLayout.GetChild(i).gameObject);
                if(roomList[i].PlayerCount == 0)
                {
                    roomList.Remove(roomList[i]);
                }
            }
        }
        foreach (var room in roomList)
        {
            GameObject newRoom = Instantiate(roomPrefab, gridLayout.position, Quaternion.identity);
            newRoom.GetComponentInChildren<Text>().text = room.Name+"("+room.PlayerCount+"people"+")";
            newRoom.transform.SetParent(gridLayout);
        }
    }
}
