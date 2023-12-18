using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDemo : MonoBehaviour
{
    public Transform camera;
    public Transform cube;
    public Transform sphere;

    public Transform currentTarget;
    public float rotationSpeed;

    public void SwapTarget()
    {
        if (currentTarget == cube)
        {
            currentTarget = sphere;
        }
        else if (currentTarget == sphere)
        {
            currentTarget = cube;
        }
    }

    private void Update()
    {
        Quaternion currentRotation = camera.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.position - camera.position);

        camera.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
