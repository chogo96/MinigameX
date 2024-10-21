using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    private TMP_Text roomInfoText;
    private RoomInfo _roomInfo;

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;

            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers})";
        }
    }

    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();     
    }

    void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        
    }
}
