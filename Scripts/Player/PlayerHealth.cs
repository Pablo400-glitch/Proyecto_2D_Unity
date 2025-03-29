using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 3;

    public delegate void UpdateHealth(int amount);
    public event UpdateHealth onUpdateHealth;
    public AudioSource audioSource;

    private int currentHealth;
    private PlayerController player;
    private Animator animator;

    private void Start()
    {
        currentHealth = playerHealth;
        player = this.GetComponent<PlayerController>();
        animator = this.GetComponent<Animator>();
    }

    public int GetHealth()
    {
        return currentHealth;
    }
    
    private void ModifyHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, playerHealth);

        if (onUpdateHealth != null)
        {
            onUpdateHealth(currentHealth);
        }
        onUpdateHealth.Invoke(currentHealth);
    }

    private void Damage()
    {
        audioSource.Play();
        ModifyHealth(-1);
    }

    private void Heal()
    {
        ModifyHealth(1);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Damage();
            animator.SetBool("Damaged", true);
            Debug.Log("Se ha dañado el jugador");
            StartCoroutine(ResetDamageAnimation(0.3f));
        }

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    private IEnumerator ResetDamageAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Damaged", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Heal"))
        {
            if (currentHealth == playerHealth)
            {
                player.InvokeMessage("Ya tienes la vida al máximo");
                return;
            }
            player.InvokeMessage("Te has curado");
            Heal();
            Destroy(other.gameObject);
        }
    }
}
