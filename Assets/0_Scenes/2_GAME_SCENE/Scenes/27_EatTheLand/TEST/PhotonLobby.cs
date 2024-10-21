using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    private Dictionary<string, GameObject> roomDic = new Dictionary<string, GameObject>();
    public GameObject roomPrefab;   //  for room list
    public Transform scrollContent;
    public Transform UI_Lobby;
    public Transform UI_CreateRoom;
    public TMP_Text UI_CreateRoom_Warning;

    public TMP_Dropdown[] gameSelects = new TMP_Dropdown[3];
    public string[] sceneNames;
    public TMP_InputField UI_Room_Name;

    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        if(roomList.Count == 0)
        {
            Debug.Log("없습니다");
        }
        else
        {
            Debug.Log("있습니다.");
        }
        GameObject tmpRoom = null;
        foreach (RoomInfo room in roomList) 
        {
            //  룸이 삭제가 된 경우
            if(room.RemovedFromList == true)
            {
                roomDic.TryGetValue(room.Name, out tmpRoom);
                Destroy(tmpRoom);
                roomDic.Remove(room.Name);
            }

            //  룸이 처음 생성된 경우
            else 
            {   
                if(roomDic.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo= room;
                    roomDic.Add(room.Name, _room);
                }
                //  룸 정보가 갱신(변경)된 경우
                else
                {
                    roomDic.TryGetValue(room.Name, out tmpRoom);
                    tmpRoom.GetComponent<RoomData>().RoomInfo= room;
                }
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
            Click_Return_At_MakeRoom();

            OnDropdownValueChanged();
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
            {
                {"CurrentRound", 0 },
                {"SelectedScenes", sceneNames }
            };

            PhotonNetwork.CreateRoom(inputNick, roomOptions);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }       
    }

    public void Click_Return_At_MakeRoom()
    {
        UI_Lobby.gameObject.SetActive(true);
        UI_CreateRoom.gameObject.SetActive(false);
        UI_CreateRoom_Warning.text = "";
    }

    public void Click_CreateUI_Button()
    {
        UI_Lobby.gameObject.SetActive(false);
        UI_CreateRoom.gameObject.SetActive(true);
    }

    void OnDropdownValueChanged()
    {
        for(int i =0; i < gameSelects.Length; i++)
        {
            sceneNames[i] = gameSelects[i].options[gameSelects[i].value].text;
        }
    }
}
