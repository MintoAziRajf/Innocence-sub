using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    SpriteRenderer switchSprite;
    [SerializeField] private Sprite offSprite = null;
    [SerializeField] private Sprite onSprite = null;
    private bool isSwitch = false;
    private void Awake()
    {
        switchSprite = GetComponent<SpriteRenderer>();
        StartCoroutine(TrapOn());
    }
    private IEnumerator TrapOn()
    {
        yield return new WaitForSeconds(1.5f);
        switchSprite.sprite = onSprite;
        GameObject.Find("Trap_Sprite").gameObject.GetComponent<Animator>().SetTrigger("TrapOn");
    }
    public void TrapOff()
    {
        if(isSwitch) return;
        switchSprite.sprite = offSprite;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.M_Switch);
        GameObject.Find("Trap_Collider").gameObject.SetActive(false);
        GameObject.Find("Trap_Sprite").gameObject.GetComponent<Animator>().SetTrigger("TrapOff");
        isSwitch = true;
    }
}
