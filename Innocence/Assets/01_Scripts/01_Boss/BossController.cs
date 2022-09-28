using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    //何ウェーブ目からスタートするか(Debug用)
    [SerializeField] private int startWave = 0;

    //攻撃開始までのフレーム数(60f = 1.0s)
    private int startDelay = 60;
    //攻撃object
    [SerializeField] private GameObject chain = null; 　　//横攻撃
    [SerializeField] private GameObject beam = null;  　　//縦攻撃
    [SerializeField] private GameObject thunder = null;  //座標攻撃

    //bossセリフ、表情差分
    [SerializeField] private SpriteRenderer bossSprite = null;
    [SerializeField] private Sprite[] bossEmote = null;
    [SerializeField] private Text bossText = null;

    [SerializeField] private GameObject goalObj = null;

    
    CSVManager csvManager; //ステージ情報を引っ張ってくる用
    //リスト
    List<string[]> bossDatas = new List<string[]>(); //ボスの挙動のリスト
    List<string[]> serifDatas = new List<string[]>(); //セリフのリスト
    //
    private int attackCount;
    [Range(0, 2)] private int attackType;
    private Vector2[] attackPos;
    private int delay;

    //1f
    private float frame = 1f / 60f;

    void Awake()
    {
        //CSVManagerからステージの情報を受け取る
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();
        bossDatas = csvManager.MainDatas;
        serifDatas = csvManager.SubDatas;
        GameObject.Find("Player").GetComponent<PlayerController>().Steps = int.Parse(bossDatas[1][5]); //bossデータから歩数を持ってくる
    }

    //BattleStartAnimation<Script>から呼び出される
    public IEnumerator BattleStart()
    {
        attackCount = bossDatas.Count;
        Debug.Log(attackCount);
        //初期待機時間
        for (int i = 0; i < startDelay; i++)
        {
            yield return new WaitForSeconds(frame);
        }
        bossText.text = serifDatas[1][0];
        //攻撃loop
        for (int i = 1; i < attackCount; i++)
        {
            //攻撃
            Attack(i);
            //攻撃クールタイム
            for (int j = 0; j < int.Parse(bossDatas[i][4]); j++)
            {
                yield return new WaitForSeconds(frame);
            }
            //表情、セリフ入れ替え
            if (i == (bossDatas.Count / 4))
            {
                bossSprite.sprite = bossEmote[0];
                bossText.text = serifDatas[2][0];
            }
            if (i == (bossDatas.Count / 2))
            {
                bossSprite.sprite = bossEmote[1];
                bossText.text = serifDatas[3][0];
            }
        }
        bossText.text = serifDatas[4][0];
        //ゴールをアクティブにする
        yield return new WaitForSeconds(0.5f);
        goalObj.SetActive(true);
    }

    //攻撃
    private void Attack(int wave)
    {
        switch (bossDatas[wave][1])
        {
            case "Horizontal":
                HorizontalAttack(wave);
                StartCoroutine(AttackSE(0));
                break;
            case "Vertical":
                VerticalAttack(wave);
                StartCoroutine(AttackSE(1));
                break;
            case "Depth":
                DepthAttack(wave);
                StartCoroutine(AttackSE(2));
                break;
        }
        //横攻撃
        void HorizontalAttack(int r)
        {
            //座標
            Vector3 pos = new Vector3(4f, int.Parse(bossDatas[r][3]), 0f);
            //生成
            Instantiate(chain, this.transform.position + pos, chain.transform.rotation, this.transform);
        }
        //縦攻撃
        void VerticalAttack(int r)
        {
            //座標
            Vector3 pos = new Vector3(int.Parse(bossDatas[r][2]), 0f, 0f);
            //生成
            Instantiate(beam, this.transform.position + pos, beam.transform.rotation, this.transform);
        }
        //2x2マス攻撃
        void DepthAttack(int r)
        {
            //座標
            Vector3 pos = new Vector3(int.Parse(bossDatas[r][2]), int.Parse(bossDatas[r][3]), 0f);
            //生成
            Instantiate(thunder, this.transform.position + pos, transform.rotation, this.transform);
        }
    }

    //攻撃効果音
    private IEnumerator AttackSE(int type)
    {
        //SE
        switch (type)
        {
            case 0:
                SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Chain_1);
                //Delay
                for (int i = 0; i < 30; i++)
                {
                    yield return new WaitForSeconds(frame);
                }
                SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Chain_2);
                break;
            case 1:
                SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Chain_1);
                for (int i = 0; i < 30; i++)
                {
                    yield return new WaitForSeconds(frame);
                }
                SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Chain_2);
                break;
            case 2:
                for (int i = 0; i < 30; i++)
                {
                    yield return new WaitForSeconds(frame);
                }
                SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Thunder);
                break;
        }
    }

    //終了
    public void EndBattle()
    {
        this.gameObject.SetActive(false);
    }
}
