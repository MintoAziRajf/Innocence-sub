using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDisplay : MonoBehaviour
{
    Animator bossAnim;
    private void Start()
    {
        bossAnim = this.GetComponent<Animator>();
    }
    public void SetPhase(int value)
    {
        bossAnim.SetInteger("Phase", value);
    }
}
