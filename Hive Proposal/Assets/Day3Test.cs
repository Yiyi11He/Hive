using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Day3Test : MonoBehaviour
{
    public TMP_InputField inputField;

    private string xrayResponse;

    public void ask_responses_xray()
    {
        inputField.text = "";
    }

    public void evaluate_responses_xray()
    {
        xrayResponse = inputField.text;
        inputField.text = "";

        // evaluate responses
    }
}
