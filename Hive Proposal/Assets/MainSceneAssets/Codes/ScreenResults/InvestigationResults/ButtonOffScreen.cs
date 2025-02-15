using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOffScreen : MonoBehaviour
{
    public GameObject targetObject; 
    public Camera customCamera; 
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (customCamera == null)
            {
                Debug.LogError("ButtonOffScreen: Custom Camera is not assigned!", this);
                return;
            }

            Ray ray = customCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) 
                {
                    ToggleTargetObject();
                }
            }
        }
    }

    void ToggleTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); 
        }
        else
        {
            Debug.LogError("ButtonOffScreen: Target Object is NULL!", this);
        }
    }
}