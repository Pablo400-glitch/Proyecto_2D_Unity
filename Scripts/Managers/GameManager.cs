using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject invisiblePlatform;
    public GameObject secretDoor;
    public BoxCollider2D endLevelCollider;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera doorCamera;

    public delegate void UpdateObjective(string objective);
    public event UpdateObjective onUpdateObjetive;

    private int remainingEnemies; // Counter for remaining enemies

    void Start()
    {
        invisiblePlatform.SetActive(false);

        // Initialize the remaining enemies counter
        remainingEnemies = enemies.Length;

        // Link each enemy to notify when it is destroyed
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
                if (enemyScript != null)
                {
                    enemyScript.SetGameManager(this);
                }
            }
        }
    }

    // Here I control the remaining enemies and trigger the events when the waves are cleared
    public void EnemyDestroyed()
    {
        remainingEnemies--;
        if (remainingEnemies == 4)
        {
            TriggerFirstWaveCleared();
        } else if (remainingEnemies <= 0)
        {
            TriggerSecondWaveCleared();
        }
    }

    // Here I trigger the first wave cleared event
    void TriggerFirstWaveCleared()
    {
        invisiblePlatform.SetActive(true);
        playerCamera.Priority = 0;
        doorCamera.Priority = 1;
        secretDoor.SetActive(false);
        StartCoroutine(ChangeCameraPriority());
        onUpdateObjetive?.Invoke("Derrota a los enemigos y encuentra la salida");
    }

    // Here I trigger the second wave cleared event
    void TriggerSecondWaveCleared()
    {
        endLevelCollider.isTrigger = true;
        onUpdateObjetive?.Invoke("Has derrotado a todos los enemigos, dirígete a la salida");
    }

    IEnumerator ChangeCameraPriority()
    {
        yield return new WaitForSeconds(2.5f);
        playerCamera.Priority = 1;
        doorCamera.Priority = 0;
    }
}
