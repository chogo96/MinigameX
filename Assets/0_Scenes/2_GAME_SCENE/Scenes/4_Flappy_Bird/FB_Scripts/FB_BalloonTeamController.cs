using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

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
    private bool hasRisen = false; // 상승이 한 번도 적용되었는지 확인
    private bool isInvincible = false; // 무적 상태 확인
    private bool isControlEnabled = true; // 조작 가능 여부

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon에 연결되지 않음. 로컬 모드로 실행됩니다.");
            isLocalMode = true;
            InitializeLocalMode();
        }
        else
        {
            InitializeTeamHoldPercentages();
        }
    }

    private void InitializeLocalMode()
    {
        teamHoldPercentages = new float[1];
        teamHoldPercentages[0] = 0f;
        teamMemberTexts[0].text = "Player 1: 0%";
    }

    public override void OnJoinedRoom()
    {
        if (!isLocalMode)
        {
            InitializeTeamHoldPercentages();
        }
    }

    private void InitializeTeamHoldPercentages()
    {
        teamHoldPercentages = new float[PhotonNetwork.PlayerList.Length];
        Debug.Log("팀 홀드 퍼센티지 배열 초기화: " + teamHoldPercentages.Length);
    }

    void Update()
    {
        if ((isLocalMode || photonView.IsMine) && isControlEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !hasRisen)
            {
                Debug.Log("스페이스바 연타 감지됨");

                int playerIndex = isLocalMode ? 0 : System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
                if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
                {
                    float spacePressValue = 20f;
                    teamHoldPercentages[playerIndex] += spacePressValue;
                    teamHoldPercentages[playerIndex] = Mathf.Clamp(teamHoldPercentages[playerIndex], 0f, 100f);

                    if (!isLocalMode)
                    {
                        photonView.RPC("UpdateTeamHoldPercentage", RpcTarget.All, playerIndex, teamHoldPercentages[playerIndex]);
                    }

                    teamMemberTexts[playerIndex].text = $"Player {playerIndex + 1}: On";
                    hasRisen = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                int playerIndex = isLocalMode ? 0 : System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
                if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
                {
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

            foreach (float percentage in teamHoldPercentages)
            {
                totalHoldPercentage += percentage;
            }

            if (teamHoldPercentages.Length > 0)
            {
                totalHoldPercentage /= teamHoldPercentages.Length;
            }
            else
            {
                totalHoldPercentage = 0f;
            }

            if (hasRisen)
            {
                currentVelocity = (totalHoldPercentage / 100f) * liftSpeed;
                hasRisen = false;
            }

            currentVelocity -= gravity * Time.deltaTime;
            Vector2 newPosition = balloonRect.anchoredPosition;
            newPosition.y += currentVelocity * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, -Screen.height / 2, Screen.height / 2);
            balloonRect.anchoredPosition = newPosition;
        }
    }

    // 무적 상태 시작 메서드
    public void TriggerInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    // 무적 상태와 조작 제한 코루틴
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        isControlEnabled = false;

        // 1초간 조작 비활성화
        yield return new WaitForSeconds(1f);
        isControlEnabled = true;

        // 3초간 무적 상태와 깜빡임 효과
        float invincibilityDuration = 3f;
        float blinkInterval = 0.2f;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (float timer = 0; timer < invincibilityDuration; timer += blinkInterval)
        {
            sprite.color = new Color(1f, 1f, 1f, 0.5f); // 반투명
            yield return new WaitForSeconds(blinkInterval / 2);
            sprite.color = new Color(1f, 1f, 1f, 1f);   // 불투명
            yield return new WaitForSeconds(blinkInterval / 2);
        }

        // 무적 상태 해제
        isInvincible = false;
        sprite.color = new Color(1f, 1f, 1f, 1f); // 불투명
    }
}
