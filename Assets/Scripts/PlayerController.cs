using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    public AudioClip coinSound;
    public AudioClip damageSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; 
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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
            if(coinSound) audioSource.PlayOneShot(coinSound);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ExitDoor"))
        {
            if (GameManager.Instance.CanExit())
            {
                GameManager.Instance.GameOver(true); // Thắng
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
            if(damageSound) audioSource.PlayOneShot(damageSound);
            
            Vector2 knockback = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockback * 10f, ForceMode2D.Impulse);
        }
    }
}