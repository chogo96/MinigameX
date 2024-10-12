using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public RectTransform[] backgrounds;  // �� �� �̻��� ��� �̹���
    public float scrollSpeed = 200f;     // ����� �⺻ ��ũ�� �ӵ�
    public RectTransform balloon;        // ���ⱸ�� RectTransform (�ӵ��� ���� ��� ��ũ��)

    private float backgroundWidth;       // ��� �ϳ��� ���� �ʺ� (���� ������ �ݿ�)

    void Start()
    {
        // ��� �ϳ��� �ʺ� ��Ȯ�� ���
        backgroundWidth = backgrounds[0].rect.width * backgrounds[0].localScale.x;
    }

    void Update()
    {
        // ���ⱸ�� �ӵ��� ���� ��� ��ũ�� �ӵ� ����
        float moveSpeed = balloon.anchoredPosition.x * Time.deltaTime;

        foreach (RectTransform background in backgrounds)
        {
            // ����� �������� �̵�
            background.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            // ����� ȭ�� ���� ������ ������ �ڷ� ���ġ
            if (background.anchoredPosition.x <= -backgroundWidth)
            {
                Vector2 newPos = background.anchoredPosition;
                newPos.x += backgroundWidth * backgrounds.Length;
                background.anchoredPosition = newPos;
            }
        }
    }
}
