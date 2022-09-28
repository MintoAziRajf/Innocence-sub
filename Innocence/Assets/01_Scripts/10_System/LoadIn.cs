using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadIn : MonoBehaviour
{
    Animator animator;
    Canvas myCanvas;
    // Start is called before the first frame update
    private void Awake()
    {
        //rednderCameraの設定
        myCanvas = GetComponent<Canvas>();
        if (myCanvas.worldCamera == null)
        {
            myCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        animator = GetComponent<Animator>();
        animator.SetTrigger("LoadIn");
    }

    private void Start()
    {
        StartCoroutine(PlaySE());
    }

    IEnumerator PlaySE()
    {  
        //SE
        yield return new WaitForSecondsRealtime(0.85f);
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Ironbars);
    }
}
