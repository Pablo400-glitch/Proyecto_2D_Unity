using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{  
    public float speed = 5f; // Speed of the player
    public float thrust = 5f; // Jump force
    public GameObject bullet;
    public float bulletSpeed;
    public float fireRate = 0.5f;   // Minimun time between each bullet instantiated

    private float nextFireTime = 0f; // Time when you can fire again
    private bool isJumping = false;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public delegate void DebugMessage(string message);
    public event DebugMessage onMessage;

    public AudioSource ShootAudio;
    public AudioSource JumpAudio;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        FireBullet();
    }

    // On this method we control the player movement
    private void Movement()
    {
        float moveH = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump") && !isJumping)
        {
            rb2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
            isJumping = true;
            JumpAudio.Play();
            animator.SetBool("Jump", true);
        }

        // When the player is ducking, it can't move
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("IsDucking", true);
            StopPlayer();
            ChangeSpriteOrientation(moveH);
            return;
        }
        else
        {
            animator.SetBool("IsDucking", false);
        }

        // When the player is shooting up, it can't move
        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            animator.SetBool("ShootUp", true);
            StopPlayer();
            ChangeSpriteOrientation(moveH);
            return;
        }
        else
        {
            animator.SetBool("ShootUp", false);
        }

        // Here I check if the player is moving to control the walking animation
        if (Mathf.Abs(moveH) > 0.01f)
        {
            Vector2 velocity = rb2D.velocity;
            velocity.x = moveH * speed;
            rb2D.velocity = velocity;

            animator.SetBool("Walk", true);
        }
        else
        {
            StopPlayer();
            animator.SetBool("Walk", false);
        }

        ChangeSpriteOrientation(moveH);
    }

    // On this method we control the player shooting
    private void FireBullet()
    {
        if (Input.GetKey(KeyCode.V)) {
            animator.SetBool("isShooting", true);
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime) {
                GameObject bullet = BulletSpawner();

                ShootAudio.Play();

                if (bullet == null)
                {
                    Debug.LogError("El objeto no tiene un componente Rigidbody2D.");
                    return;
                }
                
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (!spriteRenderer.flipX)
                    rb.velocity = transform.right * bulletSpeed; 
                else
                    rb.velocity = -transform.right * bulletSpeed; 

                if (Input.GetKey(KeyCode.W) && !isJumping)
                    rb.velocity = transform.up * bulletSpeed; 

                nextFireTime = Time.time + fireRate;   
            }
        } else {
            animator.SetBool("Stand", false);
            animator.SetBool("isShooting", false);
        }         
    }

    private GameObject BulletSpawner() {
        BulletPool pool = FindObjectOfType<BulletPool>();
        GameObject bullet = pool.GetObject();
        if (bullet != null) // Checks if the bullet is available
        {
            // Here we set the position of the bullet
            if (!spriteRenderer.flipX) {
                if (Input.GetKey(KeyCode.W) && !isJumping)
                    bullet.transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.5f, transform.position.z);
                else if (Input.GetKey(KeyCode.S))
                    bullet.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.15f, transform.position.z);
                else
                    bullet.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.15f, transform.position.z);
            }
            else {
                if (Input.GetKey(KeyCode.W) && !isJumping)
                    bullet.transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.5f, transform.position.z);
                else if (Input.GetKey(KeyCode.S))
                    bullet.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.15f, transform.position.z);
                else
                    bullet.transform.position =  new Vector3(transform.position.x - 0.5f, transform.position.y + 0.15f, transform.position.z);
            }
            bullet.GetComponent<Bullet>().setStartPosition(bullet.transform.position);
        }
        else
        {
            InvokeMessage("Sin Balas");
            return null;
        }

        return bullet;
    }

    // On this method we change the sprite orientation when the player moves left or right
    private void ChangeSpriteOrientation(float moveH)
    {
        if (moveH > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveH < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    // On this method we stop the player movement 
    private void StopPlayer()
    {
        Vector2 velocity = rb2D.velocity;
        velocity.x = 0;
        rb2D.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            animator.SetBool("Jump", false);
        }
    }

    // On this method we check if the player collides with an orb to reload the bullets
    private void OnTriggerEnter2D(Collider2D other)
    {
        BulletPool pool = FindObjectOfType<BulletPool>();
        if (other.gameObject.CompareTag("Orb"))
        {
            if (pool.GetAvailableBullets() == pool.maxBullets)
            {
                InvokeMessage("Cargador Completo");
                return;
            }
            pool.Reload(5);
            Destroy(other.gameObject);
            InvokeMessage("Balas Recargadas");
        }
    }

    public void InvokeMessage(string message)
    {
        if (onMessage != null)
        {
            onMessage(message);
        }
    }
}
