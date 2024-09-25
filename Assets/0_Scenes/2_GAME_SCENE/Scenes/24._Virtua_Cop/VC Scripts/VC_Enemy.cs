using UnityEngine;

public class VC_Enemy : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적 죽었을 때 처리
    /// </summary>
    void Die()
    {
        Destroy(gameObject);
    }
}
