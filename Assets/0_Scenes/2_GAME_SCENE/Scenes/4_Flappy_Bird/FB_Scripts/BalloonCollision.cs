using UnityEngine;
using TMPro;
using Photon.Pun;

public class BalloonCollision : MonoBehaviourPun
{
    public TMP_Text scoreText;  // 점수 표시 텍스트
    private int score = 0;      // 현재 점수

    private void Start()
    {
        UpdateScoreText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe")) // 파이프에 닿았을 때
        {
            score -= 100;
            UpdateScoreText();
        }
        else if (collision.gameObject.CompareTag("Coin")) // 동전을 먹었을 때
        {
            score += 10;
            UpdateScoreText();
            Destroy(collision.gameObject); // 동전 제거
        }
        else if (collision.gameObject.CompareTag("SpeedItem")) // 속도 아이템을 먹었을 때
        {
            GetComponent<BalloonMovement>().IncreaseSpeed(); // 열기구의 속도 증가
            Destroy(collision.gameObject); // 속도 아이템 제거
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
