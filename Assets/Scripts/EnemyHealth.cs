using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    [Header("Cấu hình Máu Quái")]
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " bị trúng đòn! Máu còn lại: " + currentHealth);

        // Hiệu ứng chớp đỏ khi bị trúng đòn (Tuỳ chọn)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
            Invoke("ResetColor", 0.1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.white;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " đã bị tiêu diệt!");
        Destroy(gameObject);
    }
}