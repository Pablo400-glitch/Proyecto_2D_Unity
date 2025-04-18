using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraNewPart : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera secondPartCamera;

    public PolygonCollider2D newPartCollider;

    private CinemachineConfiner2D confiner; 
    
    void Start()
    {
        // Get the CinemachineConfiner2D component from the player camera
        confiner = playerCamera.GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
            Debug.LogError("CinemachineConfiner2D not found on the player camera.");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Change the confiner collider to the new part collider
            if (confiner != null && newPartCollider != null)
            {
                confiner.m_BoundingShape2D = newPartCollider;
            }

            playerCamera.Priority = 0;
            secondPartCamera.Priority = 1;
            StartCoroutine(ChangeCameraPriority());
        }
    }

    IEnumerator ChangeCameraPriority()
    {
        yield return new WaitForSeconds(3f);
        playerCamera.Priority = 1;     
        secondPartCamera.Priority = 0;
    }
}
