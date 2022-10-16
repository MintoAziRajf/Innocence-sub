using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandamEffect : MonoBehaviour
{
    //エフェクトにランダムで初期回転を加えるもの
    private ParticleSystem ps;
    private void OnEnable()
    {
        ps = this.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startRotation = Random.Range(0f, 180f);
    }
}
