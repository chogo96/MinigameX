using UnityEngine;
using TMPro;
using Photon.Pun;

public class BalloonCollision : MonoBehaviourPun
{
    public TMP_Text scoreText;  // ���� ǥ�� �ؽ�Ʈ
    private int score = 0;      // ���� ����

    private void Start()
    {
        UpdateScoreText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe")) // �������� ����� ��
        {
            score -= 100;
            UpdateScoreText();
        }
        else if (collision.gameObject.CompareTag("Coin")) // ������ �Ծ��� ��
        {
            score += 10;
            UpdateScoreText();
            Destroy(collision.gameObject); // ���� ����
        }
        else if (collision.gameObject.CompareTag("SpeedItem")) // �ӵ� �������� �Ծ��� ��
        {
            GetComponent<BalloonMovement>().IncreaseSpeed(); // ���ⱸ�� �ӵ� ����
            Destroy(collision.gameObject); // �ӵ� ������ ����
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
