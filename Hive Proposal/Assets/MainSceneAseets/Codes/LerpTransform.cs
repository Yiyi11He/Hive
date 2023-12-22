using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LerpTransform : MonoBehaviour
{
    public Chair Chair;

    [Range(0f,1f)] public float value;
    public Transform MedChartStart, MedChartEnd;
    private bool isAlpha3Pressed = false;


    // Update is called once per frame
    void Update()
    {
        if (Chair.sitting == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                isAlpha3Pressed = !isAlpha3Pressed;
                value = isAlpha3Pressed ? 1f : 0f;
                transform.position = Vector3.Lerp(MedChartStart.position, MedChartEnd.position, value);
                transform.rotation = Quaternion.Slerp(MedChartStart.rotation, MedChartEnd.rotation, value);
            }
        }
        
    }
}
