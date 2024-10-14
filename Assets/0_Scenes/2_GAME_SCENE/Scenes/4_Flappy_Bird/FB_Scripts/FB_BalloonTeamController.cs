using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

public class FB_BalloonTeamController : MonoBehaviourPunCallbacks
{
    public RectTransform balloonRect;   // ���ⱸ�� RectTransform
    public float liftSpeed = 200f;      // ���ⱸ ��� �ӵ�
    public float gravity = 50f;         // �߷��� ȿ��
    public TMP_Text[] teamMemberTexts;  // ������ �Է·��� ǥ���� �ؽ�Ʈ UI �迭

    private float[] teamHoldPercentages; // ������ �Է� ���� ����
    private float totalHoldPercentage = 0f; // �� ��ü �Է� �ջ� ����
    private float currentVelocity = 0f; // ���ⱸ�� ���� �ӵ�

    private bool isLocalMode = false; // ������� ���� ���� ��� �÷���
    private bool hasRisen = false; // ����� �� ���� ����Ǿ����� Ȯ��
    private bool isInvincible = false; // ���� ���� Ȯ��
    private bool isControlEnabled = true; // ���� ���� ����

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon�� ������� ����. ���� ���� ����˴ϴ�.");
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
        Debug.Log("�� Ȧ�� �ۼ�Ƽ�� �迭 �ʱ�ȭ: " + teamHoldPercentages.Length);
    }

    void Update()
    {
        if ((isLocalMode || photonView.IsMine) && isControlEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !hasRisen)
            {
                Debug.Log("�����̽��� ��Ÿ ������");

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

    // ���� ���� ���� �޼���
    public void TriggerInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    // ���� ���¿� ���� ���� �ڷ�ƾ
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        isControlEnabled = false;

        // 1�ʰ� ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(1f);
        isControlEnabled = true;

        // 3�ʰ� ���� ���¿� ������ ȿ��
        float invincibilityDuration = 3f;
        float blinkInterval = 0.2f;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (float timer = 0; timer < invincibilityDuration; timer += blinkInterval)
        {
            sprite.color = new Color(1f, 1f, 1f, 0.5f); // ������
            yield return new WaitForSeconds(blinkInterval / 2);
            sprite.color = new Color(1f, 1f, 1f, 1f);   // ������
            yield return new WaitForSeconds(blinkInterval / 2);
        }

        // ���� ���� ����
        isInvincible = false;
        sprite.color = new Color(1f, 1f, 1f, 1f); // ������
    }
}
