using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FB_PipeSpawner : MonoBehaviour
{
    public RectTransform pipePrefab;  // ������ ������ (Canvas ������ ���)
    public RectTransform canvasRectTransform;  // Canvas�� RectTransform
    public RectTransform balloonRectTransform;  // ���ⱸ�� RectTransform (�浹 ���)
    public float spawnInterval = 2f;  // ������ ��ȯ ����
    public float minYPosition = -200f; // ������ Y�� �ּ� ��ġ (Canvas ��ǥ ����)
    public float maxYPosition = 200f;  // ������ Y�� �ִ� ��ġ (Canvas ��ǥ ����)
    public float gapSize = 200f;       // ��-�Ʒ� ������ ���� �ּ� �� ����
    public TMP_Text scoreText;  // ������ ǥ���� �ؽ�Ʈ

    private int score = 0;

    private void Start()
    {
        // �ֱ������� �������� ��ȯ
        InvokeRepeating("SpawnPipes", 1f, spawnInterval);
        UpdateScoreText();
    }

    void SpawnPipes()
    {
        // �������� ���� Y ��ġ ����
        float randomY = Random.Range(minYPosition, maxYPosition);

        // ��� ������ ����
        RectTransform topPipe = Instantiate(pipePrefab, canvasRectTransform);
        topPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY + gapSize / 2);

        // �ϴ� ������ ����
        RectTransform bottomPipe = Instantiate(pipePrefab, canvasRectTransform);
        bottomPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY - gapSize / 2);

        // ������ �̵� ����
        StartCoroutine(MovePipe(topPipe));
        StartCoroutine(MovePipe(bottomPipe));
    }

    // �������� �������� �̵��ϴ� �ڷ�ƾ
    IEnumerator MovePipe(RectTransform pipe)
    {
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            pipe.anchoredPosition += Vector2.left * 200f * Time.deltaTime; // �������� �������� �̵�

            // �浹 ����
            if (RectOverlaps(pipe, balloonRectTransform))
            {
                HandleCollision();
            }

            yield return null;
        }

        // ȭ���� ����� ������ ����
        Destroy(pipe.gameObject);
    }

    // �� RectTransform�� �浹�� �����ϴ� �Լ�
    bool RectOverlaps(RectTransform rect1, RectTransform rect2)
    {
        return rect1.rect.Overlaps(rect2.rect);
    }

    // �������� ����� �� ���� ����
    void HandleCollision()
    {
        score -= 100;
        UpdateScoreText();
    }

    // ���� ������Ʈ �Լ�
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
