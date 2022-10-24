using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCheck : MonoBehaviour
{
    [SerializeField] GameObject obj = null;
    [SerializeField ] Fade fade = null;
    // Start is called before the first frame update
    void Start()
    {
        obj.SetActive(true);
        Invoke("wait", 0.1f);
        fade.FadeIn(0, () => fade.FadeOut(3));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void wait()
    {
        obj.SetActive(false);
    }
}
