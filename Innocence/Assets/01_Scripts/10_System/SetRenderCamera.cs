using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRenderCamera : MonoBehaviour
{
    Canvas myCanvas;
    private void Awake()
    {
        //rednderCameraの設定
        myCanvas = GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        if (myCanvas.worldCamera == null)
        {
            myCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
}
