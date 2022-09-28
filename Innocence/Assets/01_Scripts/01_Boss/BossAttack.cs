using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    BattleController battleController;
    //ダメージメソッド
    protected void Damage()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Damage);
        battleController = GameObject.Find("GameManager").GetComponent<BattleController>();
        battleController.PlayerDamaged();
    }
}
