using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour
{
    CSVManager csvManager;

    private void Awake()
    {
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();
        csvManager.LoadGame();
    }
}
