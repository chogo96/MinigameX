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
    /// �� �׾��� �� ó��
    /// </summary>
    void Die()
    {
        Destroy(gameObject);
    }
}
