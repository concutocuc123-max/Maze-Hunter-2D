using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("Cấu hình Đuổi Theo (Chase)")]
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 5f; // Bán kính phát hiện Player
    public LayerMask obstacleLayer;     

    private Transform player;
    private bool isChasing = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Tự động tìm Player dựa trên Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // Tính khoảng cách tới Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Kiểm tra xem Player có nằm trong bán kính phát hiện hay không
        if (distanceToPlayer <= detectionRadius)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

            // Nếu không vướng tường -> Nhìn thấy Player và đuổi theo
            if (hit.collider == null)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else
        {
            isChasing = false; // Ra khỏi tầm nhìn -> Đứng yên lại
        }

        // Nếu phát hiện Player thì tiến hành đuổi theo
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Lật mặt quái vật theo hướng Player
        FlipSprite(player.position.x - transform.position.x);

        // Di chuyển về phía Player
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void FlipSprite(float directionX)
    {
        if (spriteRenderer != null)
        {
            if (directionX > 0.05f)
                spriteRenderer.flipX = false; // Nhìn sang phải
            else if (directionX < -0.05f)
                spriteRenderer.flipX = true;  // Nhìn sang trái
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}