using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject Player;
    public BulletPool bulletPool;
    public Text currentAmmo;
    private PlayerHealth playerHealth;

    // With this method I initialize the canvas elements
    void Start()
    {
        playerHealth = Player.GetComponent<PlayerHealth>();
        healthSlider.maxValue = playerHealth.playerHealth;
        healthSlider.value = healthSlider.maxValue;
        
        currentAmmo.text = bulletPool.maxBullets + " / " + bulletPool.maxBullets;
    }
}
