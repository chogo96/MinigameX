using UnityEngine;

public class VC_Enemy : MonoBehaviour
{
    public int health = 100; // �� �Ǵ� ������ ü��
    private bool isHit = false; // �� �Ǵ� ������ �¾Ҵ��� ����
    private VC_CursorManager cursorManager; // ���� ������ ���� ����

    void Start()
    {
        cursorManager = FindObjectOfType<VC_CursorManager>();

        if (CompareTag("VC_Enemy"))
        {
            // ���� ��� 3�� ���� ���� ������ -3�� ����
            Invoke("Missed", 3f);
        }
        else if (CompareTag("VC_Hostage"))
        {
            // ������ ��� 2�� �� �ڵ����� �����
            Destroy(gameObject, 2f);
        }
    }

    public void TakeDamage(int damage)
    {
        isHit = true; // �� �Ǵ� ������ �¾����Ƿ� isHit�� true�� ����
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// �� �Ǵ� ������ �׾��� �� ó��
    /// </summary>
    void Die()
    {
        Destroy(gameObject); // �� �Ǵ� ������ ������ ����
    }

    /// <summary>
    /// ���� 3�� ���� ���� �ʾ��� �� ȣ��Ǵ� �Լ�
    /// </summary>
    void Missed()
    {
        // ���� ���� �ʾҰ�, ���� ���� ���� ����
        if (!isHit && CompareTag("VC_Enemy"))
        {
            cursorManager.UpdateScore(-3); // ���� -3�� ����
            Destroy(gameObject); // ���� ����
        }
    }
}
