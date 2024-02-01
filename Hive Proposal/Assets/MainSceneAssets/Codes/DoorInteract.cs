using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteract : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshPro UseText;

    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;

    [SerializeField]
    private InputAction useAction;

    private void Awake()
    {
        useAction.performed += OnUseAction;
        useAction.Enable();
    }

    private void OnUseAction(InputAction.CallbackContext context)
    {
        OnUsed();
    }

    public void OnUsed()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.IsOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(transform.position);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers) && hit.collider.TryGetComponent<Door>(out Door door))
        {
            door.ShowText();
        }
    }


    /*private void Update()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers) && hit.collider.TryGetComponent<Door>(out Door door))
        {
            if (door.IsOpen)
            {
                UseText.SetText("Close \"E\"");
            }
            else
            {
                UseText.SetText("Open \"E\"");
            }
            UseText.gameObject.SetActive(true);
            UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.01f;
            //UseText.transform.rotation = Quaternion.LookRotation(hit.point - Camera.position).normalized;

            //UseText.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            UseText.gameObject.SetActive(false);

        }
    } */
}
