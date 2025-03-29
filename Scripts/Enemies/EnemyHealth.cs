using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 5.0f;
    private GameManager gameManager;
    private float currentHealth;
    public AudioSource audioSource;

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
            audioSource.Play();
            gameManager.EnemyDestroyed(); // Notify the GameManager that this enemy was destroyed
        }
    }
}
