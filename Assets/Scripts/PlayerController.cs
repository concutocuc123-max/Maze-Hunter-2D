using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    public AudioClip coinSound;
    public AudioClip damageSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Dùng cho Top-Down
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true; 
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance.AddCoin();
            if (coinSound) audioSource.PlayOneShot(coinSound);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ExitDoor"))
        {
            if (GameManager.Instance.CanExit())
            {
                GameManager.Instance.GameOver(true);
            }
            else
            {
                Debug.Log("Chưa thu thập đủ Coin!");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Trap"))
        {
            GameManager.Instance.TakeDamage(1);
            if (damageSound) audioSource.PlayOneShot(damageSound);
            
            // Đẩy lùi nhẹ (Knockback)
            Vector2 knockback = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockback * 10f, ForceMode2D.Impulse);
        }
    }
}