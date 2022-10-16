using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneClick : MonoBehaviour
{
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
    }

    public void OneClick()
    {
        Debug.Log(btn.interactable);
        btn.interactable = false;
        Debug.Log(btn.interactable);
    }
}
