using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPickup : MonoBehaviour
{
    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Transform PickupTarget;
    [Space]
    [SerializeField] private float PickupRange;
    private Rigidbody CurrentObject;

    // Expose rotation speed in the Unity editor
    [SerializeField] private float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentObject)
            {
                // Re-enable physics calculations when the object is released
                CurrentObject.isKinematic = false;
                CurrentObject.useGravity = true;
                CurrentObject = null;
                return;
            }
            Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
            {
                CurrentObject = HitInfo.rigidbody;
                // Disable physics calculations while manipulating the object
                CurrentObject.isKinematic = true;
                CurrentObject.useGravity = false;
            }
        }

        // Rotate the object
        if (CurrentObject)
        {
            RotatePickedObject();
        }
    }

    private void FixedUpdate()
    {
        if (CurrentObject)
        {
            Vector3 DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            // Manually control the position of the object when it's kinematic
            if (CurrentObject.isKinematic)
            {
                CurrentObject.MovePosition(CurrentObject.position + DirectionToPoint * Time.fixedDeltaTime * 12f * DistanceToPoint);
            }
            else
            {
                CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint;
            }
        }
    }


    private void RotatePickedObject()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0)) // Left mouse button
        {
            CurrentObject.transform.Rotate(Vector3.down, horizontalRotation, Space.World);
        }
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            CurrentObject.transform.Rotate(Vector3.up, horizontalRotation, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            CurrentObject.transform.Rotate(Vector3.right, verticalRotation, Space.World);
        }
    }
}
