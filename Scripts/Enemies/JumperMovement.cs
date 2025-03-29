using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Animations;

public class JumperMovement : MonoBehaviour
{
    public float thrust = 5f; 
    public float jumpCooldown = 2.5f; // Cooldown to control when the enemy can jump again
    public float speed = 5f;
    public float distanceOfActivation = 5f; // Activates the enemy movement when the player is near
    public Transform player;

    private float distanceFromPlayer; // Distance from the enemy where the player is
    private Rigidbody2D rb;
    private Animator animator;
    private bool isJumping;
    private float nextJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Here I check if the player is near the enemy, if so, the enemy moves towards the player jumping if he can
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceFromPlayer <= distanceOfActivation)
        {         
            MoveTowardsPlayer();      
            CheckJump();
        } else {
            StopEnemy();
        }
    }

    // The enemy moves while jumping towads the player
    void MoveTowardsPlayer() {
        Vector2 direction = (player.position - transform.position).normalized;

        Vector2 velocity = rb.velocity;
        velocity.x = direction.x * speed;
        rb.velocity = velocity;
    }

    // Checks if the enemy can jump
    void CheckJump() 
    {
        if (!isJumping && Time.time >= nextJumpTime)
        {
            Jump();           
        }
    }

    // Jump method that adds an up impulse to the enemy
    void Jump() 
    {
        isJumping = true;
        rb.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);
        animator.SetBool("Jump", true);
    }
    void StopEnemy()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);       
    }

    // If the enemy collides with the floor, it stops jumping until the cooldown is over
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            nextJumpTime = Time.time + jumpCooldown;
            animator.SetBool("Jump", false);
        }
    }
}
