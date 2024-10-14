using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameSelect : MonoBehaviourPunCallbacks
{
    [SerializeField] Button[] buttonUIs;
    [SerializeField] TextMeshProUGUI logText;
    string gameName = "";
    void Start()
    {
        // Photon 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 버튼 클릭 리스너 설정
        for (int i = 0; i < buttonUIs.Length; i++)
        {
            int index = i; // 클로저 문제를 피하기 위해 지역 변수를 사용
            buttonUIs[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void OnButtonClick(int index)
    {
        Debug.Log("Button " + index + " clicked!");

        // 마스터 서버에 접속된 상태에서만 실행
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("클릭!클릭!" + (index + 1));

            switch (index + 1)
            {
                case 1:
                    JoinRoomAndLoadScene("Mole");
                    break;
                case 3:
                    JoinRoomAndLoadScene("Plane");
                    break;
                case 9:
                    Debug.Log("클릭!클릭!");
                    JoinRoomAndLoadScene("DodgeMisile");
                    break;
                case 16:
                    JoinRoomAndLoadScene("Frogger");
                    break;
                case 26:
                    JoinRoomAndLoadScene("Flag");
                    break;
                    // 다른 case 문 추가 가능

            }
        }
        else
        {
            logText.text = "Not connected to the Master Server!";
            Debug.LogError("Not connected to the Master Server!");
        }
    }

    void JoinRoomAndLoadScene(string sceneName)
    {
        // 룸 옵션 설정 (필요에 따라 커스터마이즈 가능)
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(sceneName, roomOptions, TypedLobby.Default);
        logText.text = "Joining Room...";
        gameName = sceneName;
    }

    // Photon 서버에 성공적으로 접속했을 때 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");

        // 서버 지역 정보 추가
        string region = PhotonNetwork.CloudRegion;
        logText.text = $"Connected to Photon Master Server! Region: {region}";
    }

    // Photon 룸에 성공적으로 입장했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        int currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount; // 현재 방에 있는 플레이어 수
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers; // 방의 최대 플레이어 수

        Debug.Log($"Joined Room, loading scene... Current Players: {currentPlayers}/{maxPlayers}");
        logText.text = $"Joined Room. Current Players: {currentPlayers}/{maxPlayers}";

        // 씬 로드
        PhotonNetwork.LoadLevel(gameName);
    }

    // Photon 룸 입장 실패 시 호출되는 콜백
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        logText.text = "Failed to join room: " + message;
    }

    // Photon 서버 접속 실패 시 호출되는 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon Master Server: " + cause);
        logText.text = "Disconnected: " + cause.ToString();
    }
}
