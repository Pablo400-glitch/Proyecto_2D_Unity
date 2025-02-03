using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 5.0f;
    private GameManager gameManager;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    void Update() {
        if (currentHealth == 0) {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage() {
        currentHealth --;
    }

    void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.EnemyDestroyed(); // Notificar al GameManager
        }
    }
}
