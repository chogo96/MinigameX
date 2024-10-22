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

    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        if(roomList.Count == 0)
        {
            Debug.Log("�����ϴ�");
        }
        else
        {
            Debug.Log("�ֽ��ϴ�.");
        }
        GameObject tmpRoom = null;
        foreach (RoomInfo room in roomList) 
        {
            //  ���� ������ �� ���
            if(room.RemovedFromList == true)
            {
                roomDic.TryGetValue(room.Name, out tmpRoom);
                Destroy(tmpRoom);
                roomDic.Remove(room.Name);
            }

            //  ���� ó�� ������ ���
            else 
            {   
                if(roomDic.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo= room;
                    roomDic.Add(room.Name, _room);
                }
                //  �� ������ ����(����)�� ���
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
           

            PhotonNetwork.CreateRoom(inputNick, roomOptions);
            SceneManager.LoadScene("");
        }       
    }

    public override void OnCreatedRoom()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
            {
                {"CurrentRound", 0 },
                {"SelectedScenes", sceneNames }
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        Debug.Log("�濡 ������ �Ϸ�Ǿ����ϴ�. ���� ���� �̸��� "+ roomProperties["CurrentRound"] + roomProperties["SelectedScenes"]);
    }

    public void Click_Return_At_MakeRoom()
    {
        UI_Lobby.gameObject.SetActive(true);
        UI_CreateRoom.gameObject.SetActive(false);
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
