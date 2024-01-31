using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpV2 : MonoBehaviour
{
    public Transform pickupPoint;
    public Rigidbody targetRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            targetRb.isKinematic = true;
            targetRb.transform.parent = pickupPoint;
            targetRb.transform.localPosition = Vector3.zero;
        }
    }
}
