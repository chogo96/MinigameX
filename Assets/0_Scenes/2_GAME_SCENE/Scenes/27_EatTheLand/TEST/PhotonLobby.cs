using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    private Dictionary<string, GameObject> roomDic = new Dictionary<string, GameObject>();
    public GameObject roomPrefab;   //  for room list
    public Transform scrollContent;
    public Transform UI_Lobby;
    public Transform UI_CreateRoom;
    public TMP_Text UI_CreateRoom_Warning;

    public TMP_Dropdown dropDown1;
    public TMP_Dropdown dropDown2;
    public TMP_Dropdown dropDown3;

    public string[] sceneNames = new string[3];
    public TMP_InputField UI_Room_Name;


    PhotonView photon;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach(var room in roomList)
        {
            Debug.Log($"room = {room.Name}");

            if(room.RemovedFromList == true)
            {
                roomDic.TryGetValue(room.Name, out tempRoom );

                Destroy(tempRoom);
                roomDic.Remove(room.Name);
            }
        }
        
        
    }

    public void Click_MakeRoom_Button()
    {
        string inputNick = UI_Room_Name.text.Trim();
        if (string.IsNullOrEmpty(inputNick))
        {
            UI_CreateRoom_Warning.text = "Please input a room Name.";
        }
        else
        {
            UI_CreateRoom_Warning.text = "";
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 6;

            OnDropdownValueChanged();

            if (!PhotonNetwork.IsConnected)
            {
                Debug.LogError("Photon에 연결되어 있지 않습니다! 먼저 연결해주세요.");
                return;
            }

            Debug.Log("마스터에 들어와 있음, 방 생성 시도 중...");
            PhotonNetwork.CreateRoom(inputNick, roomOptions);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom called");
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
            {
                {"CurrentRound", 0 },
                {"SelectedScenes", sceneNames }
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        Debug.Log("방에 접속이 완료되었습니다. 현재 방의 이름은 "+ roomProperties["CurrentRound"] + roomProperties["SelectedScenes"]);
        SceneManager.LoadScene("Waiting_Room_Scene");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 접속이 완료되었습니다. ");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    public void Click_Return_At_MakeRoom()
    {
        //UI_Lobby.gameObject.SetActive(true);
        //UI_CreateRoom.gameObject.SetActive(false);
        UI_CreateRoom_Warning.text = "Waiting_Room_Scene";
    }

    public void Click_CreateUI_Button()
    {
        UI_Lobby.gameObject.SetActive(false);
        UI_CreateRoom.gameObject.SetActive(true);
    }

    void OnDropdownValueChanged()
    {
        Debug.Log(dropDown1.options.Count);

        Debug.Log(dropDown1.options[dropDown1.value].text);
        Debug.Log(dropDown1.options[dropDown2.value].text);
        Debug.Log(dropDown1.options[dropDown3.value].text);

        sceneNames[0] = dropDown1.options[dropDown1.value].text;
        sceneNames[1] = dropDown2.options[dropDown2.value].text;
        sceneNames[2] = dropDown3.options[dropDown3.value].text;
    }
}
