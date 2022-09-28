using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] lightningObj = null;
    private Animation lightningAnim;
    public void StopLightning()
    {
        for(int i = 0; i < 3; i++)
        {
            lightningAnim = lightningObj[i].GetComponent<Animation>();
            lightningAnim.Stop();
        }
    }
}
