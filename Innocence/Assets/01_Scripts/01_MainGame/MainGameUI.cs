using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] private Text screenSizeText = null;

    private void Awake()
    {
        screenSizeText.text = Screen.width + "x" + Screen.height;
    }
}
