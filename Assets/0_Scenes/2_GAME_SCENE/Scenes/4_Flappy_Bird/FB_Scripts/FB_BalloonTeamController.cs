using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class FB_BalloonTeamController : MonoBehaviourPunCallbacks
{
    public RectTransform balloonRect;
    public float liftSpeed = 200f;
    public float gravity = 50f;
    public TMP_Text[] teamMemberTexts;
    public TMP_Text scoreText;

    private float[] teamHoldPercentages;
    private float totalHoldPercentage = 0f;
    private float currentVelocity = 0f;
    private int score = 0;

    private bool isLocalMode = false;
    private bool hasRisen = false;
    public bool isInvincible = false;
    private bool isControlEnabled = true;

    public delegate void InvincibilityEndHandler();
    public event InvincibilityEndHandler OnInvincibilityEnd;
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
        //UpdateScoreText();
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

    public void TriggerInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        isControlEnabled = false;  // 스페이스바 입력 차단
        float invincibilityDuration = 3f;
        float blinkInterval = 0.2f;
        float inputDisableDuration = 1f;  // 스페이스바 입력 비활성화 시간
        Image image = GetComponent<Image>();

        // 3초 동안 깜빡이는 효과
        for (float timer = 0; timer < invincibilityDuration; timer += blinkInterval)
        {
            image.color = new Color(1f, 1f, 1f, 0.5f);  // 반투명 상태
            yield return new WaitForSeconds(blinkInterval / 2);
            image.color = new Color(1f, 1f, 1f, 1f);    // 원래 상태
            yield return new WaitForSeconds(blinkInterval / 2);

            // 깜빡임 중 1초 후 스페이스바 입력을 다시 활성화
            if (timer >= inputDisableDuration && !isControlEnabled)
            {
                isControlEnabled = true;  // 1초 후 스페이스바 입력 활성화
            }
        }

        isInvincible = false;  // 무적 상태 해제
        image.color = new Color(1f, 1f, 1f, 1f);  // 원래 색상으로 복귀

        OnInvincibilityEnd?.Invoke();  // 무적 상태 종료 이벤트 호출
    }



    public void HandleCollision()
    {
        if (!isInvincible)
        {
            score -= 100;
            TriggerInvincibility(); // 무적 상태 시작
        }
    }
}
