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

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = Player.GetComponent<PlayerHealth>();
        healthSlider.maxValue = playerHealth.playerHealth;
        healthSlider.value = healthSlider.maxValue;
        
        currentAmmo.text = bulletPool.maxBullets + " / " + bulletPool.maxBullets;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
