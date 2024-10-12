using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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

    void Start()
    {
        // Photon�� ������� �ʾ��� �� ���� ���� ��ȯ
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon�� ������� ����. ���� ���� ����˴ϴ�.");
            isLocalMode = true;
            InitializeLocalMode();
        }
        else
        {
            // �迭�� �÷��̾� ���� �°� �������� ����
            InitializeTeamHoldPercentages();
        }
    }

    // Photon�� �ƴ� ���� ��� �ʱ�ȭ
    private void InitializeLocalMode()
    {
        // ���� ��忡���� 1���� �÷��̾�� ����
        teamHoldPercentages = new float[1];
        teamHoldPercentages[0] = 0f; // ���� �÷��̾��� �Է� �� �ʱ�ȭ
        teamMemberTexts[0].text = "Player 1: 0%";
    }

    // Photon �濡 ������ �迭 �ʱ�ȭ
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
        // �迭�� �÷��̾� ���� �°� �������� ����
        teamHoldPercentages = new float[PhotonNetwork.PlayerList.Length];

        Debug.Log("�� Ȧ�� �ۼ�Ƽ�� �迭 �ʱ�ȭ: " + teamHoldPercentages.Length);
    }

    private bool hasRisen = false;

    void Update()
    {
        // ���� ��� �Ǵ� Photon ��忡�� �÷���
        if (isLocalMode || photonView.IsMine)
        {
            // �����̽��ٰ� ������ ������ ����
            if (Input.GetKeyDown(KeyCode.Space) && !hasRisen) // ����� �� ���� ������� �ʾ��� ���� ����
            {
                Debug.Log("�����̽��� ��Ÿ ������");

                int playerIndex = isLocalMode ? 0 : System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

                if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
                {
                    // �����̽��ٰ� ������ �� ������ ��°� �ο�
                    float spacePressValue = 20f; // ��Ÿ�� ������ ����� ��
                    teamHoldPercentages[playerIndex] += spacePressValue;

                    // ��°��� �ִ� 100%�� ���� �ʵ��� ����
                    teamHoldPercentages[playerIndex] = Mathf.Clamp(teamHoldPercentages[playerIndex], 0f, 100f);

                    // �ٸ� Ŭ���̾�Ʈ�� �Է� ���� (���� ��忡���� �������� ����)
                    if (!isLocalMode)
                    {
                        photonView.RPC("UpdateTeamHoldPercentage", RpcTarget.All, playerIndex, teamHoldPercentages[playerIndex]);
                    }

                    // ������ �ִ� �� �����ִ� �ؽ�Ʈ
                    teamMemberTexts[playerIndex].text = $"Player {playerIndex + 1}: On" +
                        $"";

                    // ����� �� �� ����Ǿ����� ǥ��
                    hasRisen = true;
                }
            }

            // �����̽��ٰ� ������ �ʾ��� �� �ؽ�Ʈ�� Off�� ������Ʈ
            if (Input.GetKeyUp(KeyCode.Space))
            {
                int playerIndex = isLocalMode ? 0 : System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

                if (playerIndex >= 0 && playerIndex < teamHoldPercentages.Length)
                {
                    // �ؽ�Ʈ�� Off�� ������Ʈ
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
            Debug.Log("�����̽� �� RPC ����");

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

            // �� ��ü �Է� �ջ� ���
            foreach (float percentage in teamHoldPercentages)
            {
                totalHoldPercentage += percentage;
            }

            // ���� ���� ������ ��� �Է� ���� ���
            if (teamHoldPercentages.Length > 0)
            {
                totalHoldPercentage /= teamHoldPercentages.Length;
            }
            else
            {
                totalHoldPercentage = 0f; // ������ ���� 0���� ����
            }

            // ��·� ���: �����̽��ٸ� �� �� ���� �������� ��·��� �����
            if (hasRisen)
            {
                currentVelocity = (totalHoldPercentage / 100f) * liftSpeed;
                hasRisen = false; // ��·��� �� ���� ����ǵ��� ����
            }

            // �߷� ���� (�����̽��ٸ� ���� �Ŀ��� �߷¸� ����)
            currentVelocity -= gravity * Time.deltaTime;

            // �߷� ���� �� NaN ����
            if (float.IsNaN(currentVelocity))
            {
                currentVelocity = 0f;
            }

            Vector2 newPosition = balloonRect.anchoredPosition;
            newPosition.y += currentVelocity * Time.deltaTime;

            // ȭ�� ���� ������ ���� (NaN ����)
            if (!float.IsNaN(newPosition.y))
            {
                newPosition.y = Mathf.Clamp(newPosition.y, -Screen.height / 2, Screen.height / 2);
                balloonRect.anchoredPosition = newPosition;
            }
        }
    }
}
