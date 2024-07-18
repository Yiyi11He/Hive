using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnChoiceManager : MonoBehaviour
{
    [YarnCommand("SelectInfected")]
    public void SelectInfected()
    {
        SetYarnVariable("$Infected", true);
    }

    [YarnCommand("SelectErythema")]
    public void SelectErythema()
    {
        SetYarnVariable("$Erythema", true);
    }

    [YarnCommand("SelectSwelling")]
    public void SelectSwelling()
    {
        SetYarnVariable("$Swelling", true);
    }

    [YarnCommand("SelectUlcer")]
    public void SelectUlcer()
    {
        SetYarnVariable("$Ulcer", true);
    }

    [YarnCommand("SelectDiabetic")]
    public void SelectDiabetic()
    {
        SetYarnVariable("$Diabetic", true);
    }

    [YarnCommand("SelectPlantar")]
    public void SelectPlantar()
    {
        SetYarnVariable("$Plantar", true);
    }

    [YarnCommand("SelectVenous")]
    public void SelectVenous()
    {
        SetYarnVariable("$Venous", true);
    }

    [YarnCommand("SelectArterial")]
    public void SelectArterial()
    {
        SetYarnVariable("$Arterial", true);
    }

    [YarnCommand("SelectPurple")]
    public void SelectPurple()
    {
        SetYarnVariable("$Purple", true);
    }

    [YarnCommand("SelectPus")]
    public void SelectPus()
    {
        SetYarnVariable("$Pus", true);
    }

    private void SetYarnVariable(string variableName, bool value)
    {
        var variableStorage = FindObjectOfType<VariableStorageBehaviour>();
        if (variableStorage != null)
        {
            variableStorage.SetValue(variableName, value);
        }
    }
}