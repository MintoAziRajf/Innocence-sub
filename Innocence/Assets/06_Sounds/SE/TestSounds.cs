using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Shutter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
