using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickEffect : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    public void OnKick()
    {
        anim.SetTrigger("Kick");
    }
}
