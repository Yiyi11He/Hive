using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBack : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
