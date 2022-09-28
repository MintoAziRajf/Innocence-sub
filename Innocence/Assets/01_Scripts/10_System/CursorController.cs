using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //マウスカーソルの無効化
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}