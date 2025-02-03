using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJoint : MonoBehaviour
{
    public DistanceJoint2D joint;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            joint.gameObject.SetActive(true);
            joint.connectedBody = player.GetComponent<Rigidbody2D>();
        }
    }
}
