using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject player;
    public Slider healthSlider;
    public GameObject message;
    public BulletPool bulletPool;
    public Text bulletCounter;
    public Text objetiveText;
    public GameManager gameManager;

    private Text messageText;
    private Image messageImage;
    private PlayerHealth playerHealth;
    private PlayerController playerController;

    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        playerController = player.GetComponent<PlayerController>();

        playerHealth.onUpdateHealth += UpdateHealthCounter;
        playerController.onMessage += ShowMessage;
        bulletPool.onUpdateAmmo += UpdateAmmoCounter;
        gameManager.onUpdateObjetive += UpdateObjetive;

        messageText = message.GetComponentInChildren<Text>();
        messageImage = message.GetComponentInChildren<Image>();
        messageImage.gameObject.SetActive(false);
    }

    void UpdateHealthCounter(int Health)
    {
        healthSlider.value = Health;
    }

    void UpdateAmmoCounter(int Ammo)
    {
        bulletCounter.text = Ammo + " / " + bulletPool.maxBullets;
    }

    void UpdateObjetive(string objetive)
    {
        objetiveText.text = objetive;
    }

    void ShowMessage(string message)
    {
        messageText.text = message;
        messageImage.gameObject.SetActive(true);
        StartCoroutine(HideMessage());           
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2.5f);
        messageText.text = "";
        messageImage.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        playerHealth.onUpdateHealth -= UpdateHealthCounter;
        playerController.onMessage -= ShowMessage;
        bulletPool.onUpdateAmmo -= UpdateAmmoCounter;
        gameManager.onUpdateObjetive -= UpdateObjetive;
    }
}
