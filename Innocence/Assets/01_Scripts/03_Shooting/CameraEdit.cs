using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEdit : MonoBehaviour
{
    private Camera cam;
    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.nearClipPlane = 9f;
        StartCoroutine(ChangeClipping());
    }
    
    IEnumerator ChangeClipping()
    {
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 60; i++)
        {
            yield return null;
            cam.nearClipPlane -= 6.0f / 60f;
        }
        
    }
}
