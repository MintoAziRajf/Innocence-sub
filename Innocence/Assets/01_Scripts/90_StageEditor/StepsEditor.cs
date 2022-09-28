using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsEditor : MonoBehaviour
{
    private int steps = 0;
    private Text stepsText = null;

    private void Awake()
    {
        stepsText = GetComponent<Text>();
    }

    private void UpdateSteps()
    {
        stepsText.text = steps.ToString("00");
    }

    public void StepsUp()
    {
        if (steps < 99) steps++;
        UpdateSteps();
    }
    public void StepsDown()
    {
        if (steps > 0) steps--;
        UpdateSteps();
    }
}
