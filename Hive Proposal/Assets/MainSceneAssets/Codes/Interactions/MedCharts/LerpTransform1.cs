using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTransform1 : MonoBehaviour
{
    public Chair Chair;
    public int activeObject;
    
    [Range(0f, 1f)] public float value;
    public Transform MedChartStart, MedChartEnd;
    private bool isAlpha1Pressed = false;
    private bool isAlpha2Pressed = false;
    private bool isAlpha3Pressed = false;
    private float targetValue = 0f;
    private float transitionSpeed = 0.05f; 

    // Update is called once per frame
    void Update()
    {
        if (Chair.sitting == true)
        {
            if (activeObject == 1) HandleKeyPress(KeyCode.Alpha1, ref isAlpha1Pressed);
            if (activeObject == 2) HandleKeyPress(KeyCode.Alpha2, ref isAlpha2Pressed);
            if (activeObject == 3) HandleKeyPress(KeyCode.Alpha3, ref isAlpha3Pressed);
        }
    }

    private void HandleKeyPress(KeyCode key, ref bool isKeyPressed)
    {
        if (Input.GetKeyDown(key))
        {
            isKeyPressed = !isKeyPressed;
            targetValue = isKeyPressed ? 1f : 0f;
        }

        // Gradually change value over time
        if (value < targetValue)
        {
            value += transitionSpeed;
            if (value > targetValue) value = targetValue; 
        }
        else if (value > targetValue)
        {
            value -= transitionSpeed;
            if (value < targetValue) value = targetValue; 
        }

        transform.position = Vector3.Lerp(MedChartStart.position, MedChartEnd.position, value);
        transform.rotation = Quaternion.Slerp(MedChartStart.rotation, MedChartEnd.rotation, value);
    }
}
