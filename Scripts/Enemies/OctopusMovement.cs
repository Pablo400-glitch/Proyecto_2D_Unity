using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusMovement : MonoBehaviour
{
    public float thrust = 8f; 
    public float jumpCooldown = 2.5f; // Cooldown to control when the enemy can jump again
  
    private Rigidbody2D rb;
    private Animator animator;
    private bool isJumping;
    private float nextJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isJumping && Time.time >= nextJumpTime)
        {
            Jump();
        }
    }

    void Jump() 
    {
        isJumping = true;
        rb.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);
        animator.SetBool("Jump", true);
    }

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
