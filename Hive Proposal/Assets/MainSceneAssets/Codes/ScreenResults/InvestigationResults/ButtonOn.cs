using UnityEngine;

public class ButtonOn : MonoBehaviour
{
    public GameObject targetObject; 
    public Camera customCamera; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (customCamera == null)
            {
                Debug.LogError("ButtonOn: Custom Camera is not assigned!", this);
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
            targetObject.SetActive(!targetObject.activeSelf); 
        }
        else
        {
            Debug.LogError("ButtonOn: Target Object is NULL!", this);
        }
    }
}