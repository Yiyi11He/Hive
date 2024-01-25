using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSitting : MonoBehaviour
{
    public Chair chair;

    public Transform camerA;
    public Transform computer;
    public Transform charts;

    public Transform currentTarget;
    public float rotationSpeed;

    public void SwapTarget()
    {
        if (currentTarget == computer)
        {
            currentTarget = charts;
        }
        else if (currentTarget == charts)
        {
            currentTarget = computer;
        }
    }

    private void Update()
    {
        Quaternion currentRotation = camerA.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.position - camerA.position);

        camerA.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
