using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthAttack : BossAttack
{
    private Color c = new Color(1f, 1f, 1f, 0f);
    private float alpha = 0f;
    private Vector3 markScale = new Vector3(2.4f, 2.4f, 1f);

    [SerializeField] private GameObject thunder = null;
    [SerializeField] private GameObject mark = null;
    [SerializeField, Tooltip("攻撃までのフレーム数")] private int delay = 0;

    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private void OnEnable()
    {
        //Init
        c.a = alpha;
        sr = mark.GetComponent<SpriteRenderer>();
        sr.enabled = true;
        bc = this.GetComponent<BoxCollider2D>();
        bc.enabled = false;
        //攻撃メソッド
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        for (int i = 0; i < delay; i++)
        {
            //不透明度調整
            alpha += 1f / (float)delay;
            c.a = alpha;
            sr.color = c;
            yield return new WaitForSeconds(0.01667f);
        }
        //マークの削除
        alpha = 0f;
        c.a = alpha;
        sr.color = c;
        //コライダーのアクティブ
        bc.enabled = true;
        //落雷のアクティブ
        thunder.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            if (i == 15) bc.enabled = false;
            yield return new WaitForSeconds(0.01667f);
        }
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
