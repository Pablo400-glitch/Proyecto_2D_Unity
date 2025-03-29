using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private float maxDistance = 2.5f; 
    private Vector3 startPosition;
    private BulletPool pool; // Pool reference

    void Start()
    {
        startPosition = transform.position;
        pool = FindObjectOfType<BulletPool>(); // Finds the pool in the scene
    }

    public void setStartPosition(Vector3 position)
    {
        startPosition = position;
    }

    void Update()
    {
        float traveledDistance = Vector3.Distance(startPosition, transform.position);

        if (traveledDistance >= maxDistance)
        {
            pool.ReturnObject(gameObject); // Returns the bullet to the pool
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.transform.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.GetComponent<EnemyHealth>();
            health.TakeDamage();
        }

        // Checks if the bullet collides with the border of the camera confiner
        if (collision.name != "Border")
        {
            // Returns the bullet to the pool if it collides with something different than the enemy collider
            if (!collision.gameObject.CompareTag("EnemyCollider"))
            {
                Debug.Log("Colisión con: " + collision.name);
                pool.ReturnObject(gameObject);
            }
        }
    }
}
