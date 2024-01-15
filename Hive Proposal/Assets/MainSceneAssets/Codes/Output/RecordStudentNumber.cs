using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordStudentNumber : MonoBehaviour
{
    [SerializeField] private ResultsData resultsData;
    [SerializeField] private TMP_InputField textField;

    public void Record()
    {
        resultsData.Clear();
        resultsData.SetStudentNumber(textField.text);
    }
}
