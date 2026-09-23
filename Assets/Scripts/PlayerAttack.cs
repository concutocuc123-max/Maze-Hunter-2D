using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [Header("Cấu hình Tấn công")]
    public float attackRange = 1.5f;    
    public int attackDamage = 1;        
    public float attackCooldown = 0.4f; 
    public LayerMask enemyLayer;      

    public AudioClip attackSound;      
    private float nextAttackTime = 0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Nhấn CHUỘT TRÁI (0) để tấn công
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))
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