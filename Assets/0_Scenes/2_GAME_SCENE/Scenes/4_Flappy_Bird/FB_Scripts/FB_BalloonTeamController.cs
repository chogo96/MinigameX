using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FB_BalloonTeamController : MonoBehaviourPunCallbacks
{
    public RectTransform balloonRect;   // 열기구의 RectTransform
    public float liftSpeed = 200f;      // 열기구 상승 속도
    public float gravity = 50f;         // 중력의 효과
    public TMP_Text[] teamMemberTexts;  // 팀원의 입력량을 표시할 텍스트 UI 배열

    private float[] teamHoldPercentages; // 팀원의 입력 비율 저장
    private float totalHoldPercentage = 0f; // 팀 전체 입력 합산 비율
    private float currentVelocity = 0f; // 열기구의 현재 속도

    private bool isLocalMode = false; // 디버깅을 위한 로컬 모드 플래그

    void Start()
    {
        // Photon에 연결되지 않았을 때 로컬 모드로 전환
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon에 연결되지 않음. 로컬 모드로 실행됩니다.");
            isLocalMode = true;
            InitializeLocalMode();
        }
        else
        {
            // 배열을 플레이어 수에 맞게 동적으로 설정
            InitializeTeamHoldPercentages();
        }
    }

    // Photon이 아닌 로컬 모드 초기화
    private void InitializeLocalMode()
    {
        // 로컬 모드에서는 1명의 플레이어로 간주
        teamHoldPercentages = new float[1];
        teamHoldPercentages[0] = 0f; // 로컬 플레이어의 입력 값 초기화
        teamMemberTexts[0].text = "Player 1: 0%";
    }

    // Photon 방에 들어오면 배열 초기화
    public override void OnJoinedRoom()
    {
        if (!isLocalMode)
        {
            InitializeTeamHoldPercentages();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!isLocalMode)
        {
            InitializeTeamHoldPercentages();
        }
    }

    private void InitializeTeamHoldPercentages()
    {
        // 배열을 플레이어 수에 맞게 동적으로 설정
        teamHoldPercentages = new float[PhotonNetwork.PlayerList.Length];

        Debug.Log("팀 홀드 퍼센티지 배열 초기화: " + teamHoldPercentages.Length);
    }

    private bool hasRisen = false;

    void Update()
    {
        // 로컬 모드 또는 Photon 모드에서 플레이
        if (isLocalMode || photonView.IsMine)
        {
            // 스페이스바가 눌리는 순간만 감지
            if (Input.GetKeyDown(KeyCode.Space) && !hasRisen) // 상승이 한 번도 적용되지 않았을 때만 실행
            {
                Debug.Log("스페이스바 연타 감지됨");

                int playerIndex = isLocalMode ? 0 : System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

                if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
                {
                    // 스페이스바가 눌렸을 때 일정한 상승값 부여
                    float spacePressValue = 20f; // 연타할 때마다 상승할 양
                    teamHoldPercentages[playerIndex] += spacePressValue;

                    // 상승값이 최대 100%를 넘지 않도록 제한
                    teamHoldPercentages[playerIndex] = Mathf.Clamp(teamHoldPercentages[playerIndex], 0f, 100f);

                    // 다른 클라이언트에 입력 전송 (로컬 모드에서는 실행하지 않음)
                    if (!isLocalMode)
                    {
                        photonView.RPC("UpdateTeamHoldPercentage", RpcTarget.All, playerIndex, teamHoldPercentages[playerIndex]);
                    }

                    // 동력을 주는 지 보여주는 텍스트
                    teamMemberTexts[playerIndex].text = $"Player {playerIndex + 1}: On" +
                        $"";

                    // 상승이 한 번 적용되었음을 표시
                    hasRisen = true;
                }
            }

            // 스페이스바가 눌리지 않았을 때 텍스트를 Off로 업데이트
            if (Input.GetKeyUp(KeyCode.Space))
            {
                int playerIndex = isLocalMode ? 0 : System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

                if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
                {
                    // 텍스트를 Off로 업데이트
                    teamMemberTexts[playerIndex].text = $"Player {playerIndex + 1}: Off";
                }
            }
        }
    }

    [PunRPC]
    void UpdateTeamHoldPercentage(int playerIndex, float percentage)
    {
        if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
        {
            Debug.Log("스페이스 바 RPC 눌림");

            teamHoldPercentages[playerIndex] = percentage;

            if (photonView.IsMine)
            {
                teamMemberTexts[playerIndex].text = $"Player {playerIndex + 1}: {percentage.ToString("F0")}%";
            }
        }
    }

    void FixedUpdate()
    {
        if (isLocalMode || photonView.IsMine)
        {
            totalHoldPercentage = 0f;

            // 팀 전체 입력 합산 계산
            foreach (float percentage in teamHoldPercentages)
            {
                totalHoldPercentage += percentage;
            }

            // 팀원 수로 나누어 평균 입력 비율 계산
            if (teamHoldPercentages.Length > 0)
            {
                totalHoldPercentage /= teamHoldPercentages.Length;
            }
            else
            {
                totalHoldPercentage = 0f; // 안전을 위해 0으로 설정
            }

            // 상승력 계산: 스페이스바를 한 번 누른 순간에만 상승력이 적용됨
            if (hasRisen)
            {
                currentVelocity = (totalHoldPercentage / 100f) * liftSpeed;
                hasRisen = false; // 상승력이 한 번만 적용되도록 설정
            }

            // 중력 적용 (스페이스바를 누른 후에는 중력만 적용)
            currentVelocity -= gravity * Time.deltaTime;

            // 중력 적용 후 NaN 방지
            if (float.IsNaN(currentVelocity))
            {
                currentVelocity = 0f;
            }

            Vector2 newPosition = balloonRect.anchoredPosition;
            newPosition.y += currentVelocity * Time.deltaTime;

            // 화면 범위 내에서 제한 (NaN 방지)
            if (!float.IsNaN(newPosition.y))
            {
                newPosition.y = Mathf.Clamp(newPosition.y, -Screen.height / 2, Screen.height / 2);
                balloonRect.anchoredPosition = newPosition;
            }
        }
    }
}
