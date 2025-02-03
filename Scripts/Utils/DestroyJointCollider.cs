using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyJointCollider : MonoBehaviour
{
    public GameObject JointCollider;
    
    void OnDestroy()
    {
        Destroy(JointCollider);
    }
}
