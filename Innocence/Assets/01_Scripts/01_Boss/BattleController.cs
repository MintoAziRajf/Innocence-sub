using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    //プレイヤーのHP
    private int playerHp = 4;
    //HP表示用
    [SerializeField] private GameObject hpObj = null;
    [SerializeField] private GameObject[] hpDisplay = null;
    //画面の揺れ
    [SerializeField] private GameObject battleCamera = null;
    [SerializeField] private float magnitude = 0f;
    //DamageEffect
    [SerializeField] private Image damageScreen = null;
    //無敵時間
    private float invisibleTime;

    private void Update()
    {
        //無敵時間
        if (invisibleTime < 0f) return;
        invisibleTime -= Time.deltaTime;
    }
    
    public void PlayerDamaged()
    {
        //エフェクト
        StartCoroutine(Shake());
        StartCoroutine(DamageFlash());
        //無敵時間
        if (invisibleTime > 0f) return;
        invisibleTime = 0.2f;
        //Hpを減らしHpの残数を画面に表示
        playerHp--;
        if (playerHp == 0)
        {
            this.GetComponent<MainGameManager>().GameOver();
        }
        Destroy(hpDisplay[playerHp]);
    }

    //ダメージを受けたときに画面を赤く点滅させる
    private IEnumerator DamageFlash()
    {
        float alpha = 0f;
        Color c = damageScreen.color;
        for (int i = 0; i < 6; i++)
        {
            yield return null;
            //不透明度計算
            alpha += 0.015f;
            c.a = alpha;
            damageScreen.color = c;
        }
        for (int i = 0; i < 6; i++)
        {
            yield return null;
            //不透明度計算
            alpha -= 0.015f;
            c.a = alpha;
            damageScreen.color = c;
        }
        alpha = 0f;
        c.a = alpha;
        damageScreen.color = c;
    }

    //ダメージを受けたときに画面を揺らす
    private IEnumerator Shake()
    {
        //カメラの初期位置
        var pos = battleCamera.transform.localPosition;
        var pos2 = hpObj.transform.localPosition;

        var elapsed = 0f;

        while (elapsed < 0.2f)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;
            var x2 = pos2.x + Random.Range(-100f, 100f) * magnitude;
            var y2 = pos2.y + Random.Range(-100f, 100f) * magnitude;

            battleCamera.transform.localPosition = new Vector3(x, y, pos.z);
            hpObj.transform.localPosition = new Vector3(x2, y2, pos2.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        battleCamera.transform.localPosition = pos;
        hpObj.transform.localPosition = pos2;
    }
}
