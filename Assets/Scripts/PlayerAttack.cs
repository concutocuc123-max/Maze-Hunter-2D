using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [Header("Cấu hình Tấn công")]
    public float attackRange = 1.5f;     // Bán kính vùng tấn công xung quanh Player
    public int attackDamage = 1;        // Sát thương gây ra mỗi lần bấm Space
    public float attackCooldown = 0.4f; // Thời gian chờ giữa 2 lần đánh 
    public LayerMask enemyLayer;        // Layer của Quái vật

    public AudioClip attackSound;       // Âm thanh vung đòn 
    private float nextAttackTime = 0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Nhấn phím SPACE để tấn công
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        if (attackSound && audioSource) audioSource.PlayOneShot(attackSound);

        // Quét tất cả Collider của Enemy nằm trong vòng tròn tầm đánh
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}