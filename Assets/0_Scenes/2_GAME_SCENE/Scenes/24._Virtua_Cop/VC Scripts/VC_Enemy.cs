using UnityEngine;

public class VC_Enemy : MonoBehaviour
{
    public int health = 100; // 적 또는 인질의 체력
    private bool isHit = false; // 적 또는 인질이 맞았는지 여부
    private VC_CursorManager cursorManager; // 점수 관리를 위한 참조

    void Start()
    {
        cursorManager = FindObjectOfType<VC_CursorManager>();

        if (CompareTag("VC_Enemy"))
        {
            // 적인 경우 3초 동안 맞지 않으면 -3점 차감
            Invoke("Missed", 3f);
        }
        else if (CompareTag("VC_Hostage"))
        {
            // 인질인 경우 2초 후 자동으로 사라짐
            Destroy(gameObject, 2f);
        }
    }

    public void TakeDamage(int damage)
    {
        isHit = true; // 적 또는 인질이 맞았으므로 isHit를 true로 설정
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적 또는 인질이 죽었을 때 처리
    /// </summary>
    void Die()
    {
        Destroy(gameObject); // 적 또는 인질이 죽으면 제거
    }

    /// <summary>
    /// 적이 3초 동안 맞지 않았을 때 호출되는 함수
    /// </summary>
    void Missed()
    {
        // 적이 맞지 않았고, 적일 때만 점수 차감
        if (!isHit && CompareTag("VC_Enemy"))
        {
            cursorManager.UpdateScore(-3); // 점수 -3점 차감
            Destroy(gameObject); // 적을 제거
        }
    }
}
