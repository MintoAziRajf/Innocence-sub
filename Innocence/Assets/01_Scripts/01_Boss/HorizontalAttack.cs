using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAttack : BossAttack
{
    //当たり判定の有無の色
    private Color inactiveColor = new Color(1f,1f,1f, 0.2f);
    private Color activeColor = new Color(1f,1f,1f,1f);

    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Vector3 scale = new Vector3(1f, 1f, 1f);

    [SerializeField,Tooltip("警告フレーム数")] private int startDelay = 0;
    [SerializeField, Tooltip("攻撃持続フレーム数")] private int delay = 0;

    private void OnEnable()
    {
        //Init
        bc = this.GetComponent<BoxCollider2D>();
        bc.enabled = false;
        sr = this.GetComponent<SpriteRenderer>();
        sr.color = inactiveColor;

        //攻撃メソッド
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        //警告
        for (int i = 0; i < startDelay; i++)
        {
            yield return new WaitForSeconds(1f/60f);
            scale.x = scale.x - (1f / (float)startDelay);
            this.transform.localScale = scale;
        }
        //Scaleを戻す
        scale.x = 1f;
        //当たり判定アクティブ
        bc.enabled = true;
        sr.color = activeColor;
        //攻撃
        for (int i = 0; i < delay; i++)
        {
            yield return new WaitForSeconds(1f / 60f);
            scale.x = scale.x - (1f / (float)delay);
            this.transform.localScale = scale;
        }
        //終了後削除
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damage();
        }
    }
}
