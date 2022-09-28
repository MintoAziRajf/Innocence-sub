using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyEditor : MonoBehaviour
{
    private int difficulty = 0;
    private Text difficultyText = null;

    private void Awake()
    {
        difficultyText = GetComponent<Text>();
    }

    private void UpdateDifficulty()
    {
        difficultyText.text = difficulty.ToString("0");
    }

    public void DifficultyUp()
    {
        if (difficulty < 9) difficulty++;
        UpdateDifficulty();
    }
    public void DifficultyDown()
    {
        if (difficulty > 0) difficulty--;
        UpdateDifficulty();
    }
}
