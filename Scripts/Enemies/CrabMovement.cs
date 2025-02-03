using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isWalking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 movement = new Vector2(-speed, rb.velocity.y);
        rb.velocity = movement;

        isWalking = (rb.velocity.x != 0 || rb.velocity.y != 0) ? true : false;
        animator.SetBool("Walk", isWalking);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyCollider"))
        {
            speed = -speed;

            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }
}
