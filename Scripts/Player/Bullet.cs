using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private float maxDistance = 2.5f; 
    private Vector3 startPosition;
    private BulletPool pool; // Referencia al pool

    void Start()
    {
        startPosition = transform.position;
        pool = FindObjectOfType<BulletPool>(); // Encuentra el pool en la escena
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
            pool.ReturnObject(gameObject); // Devuelve la bala al pool
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.transform.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.GetComponent<EnemyHealth>();
            health.TakeDamage();
        }
        if (collision.name != "Border")
        {
            if (!collision.gameObject.CompareTag("EnemyCollider"))
            {
                Debug.Log("Colisión con: " + collision.name);
                pool.ReturnObject(gameObject); // Devuelve la bala al pool
            }
        }
    }
}
