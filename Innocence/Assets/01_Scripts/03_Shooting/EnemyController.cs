using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //ダメージエフェクト
    [SerializeField] private GameObject hitEffect = null;
    [SerializeField] private GameObject destroyEffect = null;
    //クリアアニメーション
    [SerializeField] private GameObject completeCanvas = null;

    //移動
    private Vector3 targetPos;
    private float speed = 1f;

    //攻撃タイプ
    [SerializeField] private int type = 0;

    //ステータス
    private int hp = 40;
    //hp表示用
    [SerializeField] private GameObject[] hpWall = null;
    [ColorUsage(false, true)] Color firstColor = new Color(0.3f, 2.5f, 4.0f); //blue
    [ColorUsage(false, true)] Color secondColor = new Color(0.15f, 1.5f, 0.25f);//green
    [ColorUsage(false, true)] Color thirdColor = new Color(1.5f, 1.5f, 0.15f); //yellow
    [ColorUsage(false, true)] Color fourthColor = new Color(3.0f, 0.3f, 0.3f);//red
    [ColorUsage(false, false)] Color damagedColor = new Color(0.23f, 0.23f, 0.23f);//gray

    //射撃
    private float interval = 0.3f;
    private float time;
    private float power = 150f;

    //弾
    GameObject b;
    Rigidbody rb;
    [SerializeField] private GameObject defaultBullet = null;
    [SerializeField] private GameObject specialBullet = null;
    private bool isDefault = true;

    //角度
    Vector3 angle;
    Vector3 angleForward = Vector3.forward;

    //player
    private GameObject player;

    //GameManager
    MainGameManager mGameManager;

    //ShootingManager
    ShootingManager sm;

    //フラグ
    private bool isStart = false;
    public bool IsStart
    {
        get { return this.isStart; }
        set { this.isStart = value; }
    }

    void OnEnable()
    {
        //Init
        player = GameObject.Find("Player");
        sm = GameObject.Find("ShootingManager").GetComponent<ShootingManager>();
        mGameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();
        type = mGameManager.Difficulty;
        SetEnemyStatus();
        
        targetPos = this.transform.position;
    }

    private void SetEnemyStatus()
    {
        speed = 1.0f + (type * 0.1f); //typeに応じて移動速度を変える
        power = 150f + (type * 10f);
        interval = 0.2f + (type * 0.02f);
    }

    void FixedUpdate()
    {
        if (!isStart) return;
        SetEnemyStatus();
        //playerの方を向く
        LookAtPlayer();
        //Movement
        Move();
        //射撃インターバル
        time += Time.deltaTime;
        if (time < interval) return;
        time = 0f;

        switch (type)
        {
            case 0:
                AttackTypeA();
                break;
            case 1:
                AttackTypeB();
                break;
            case 2:
                AttackTypeC();
                break;
            case 3:
                AttackTypeD();
                break;
            case 4:
                AttackTypeE();
                break;
            case 5:
                AttackTypeF();
                break;
            case 6:
                AttackTypeG();
                break;
            case 7:
                AttackTypeH();
                break;
            case 8:
                AttackTypeI();
                break;
            case 9:
                AttackTypeJ();
                break;
        }
        void Move()
        {
            
            //移動
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            //目的地についていなければ何もしない
            if (!(this.transform.position == targetPos)) return;

            //目的地をセット
            targetPos.x = Random.Range(-4.0f, 4.0f);
            targetPos.z = Random.Range(-4.0f, 4.0f);
        }

        void AttackTypeA() //追従　通常弾
        {
            angle = transform.forward;
            Shot(true);
        }
        void AttackTypeB() //3発　通常弾
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, -15f, 0f);
            angle = q.normalized * transform.forward;
            q = Quaternion.Euler(0f, 15f, 0f);

            //射撃
            for (int i = 0; i < 3; i++)
            {
                Shot(true);
                angle = q.normalized * angle;
            }
        }
        void AttackTypeC() //追従　弾交互
        {
            angle = transform.forward;
            //弾の生成・発射
            Shot(isDefault);

            //弾切り替え
            isDefault = !isDefault;
        }
        void AttackTypeD() //回転　弾交互
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, 45f, 0f);
            Quaternion q2 = Quaternion.Euler(0f, 30f, 0f);
            //初弾の角度
            angleForward = q2.normalized * angleForward;
            angle = angleForward;

            for (int i = 0; i < 8; i++)
            {
                isDefault = (i <= 3);
                angle = q.normalized * angle;
                Shot(isDefault);
            }
        }
        void AttackTypeE() //トリニティ　交互
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, -45f, 0f);
            angle = q.normalized * transform.forward;
            q = Quaternion.Euler(0f, 15f, 0f);

            //射撃
            for (int i = 0; i < 7; i++)
            {
                Shot(isDefault);
                angle = q.normalized * angle;
            }
            isDefault = !isDefault;
        }
        void AttackTypeF() //回転　特殊弾のみ
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, 90f, 0f);
            Quaternion q2 = Quaternion.Euler(0f, 10f, 0f);
            //初弾の角度
            angleForward = q2.normalized * angleForward;
            angle = angleForward;

            //射撃
            for (int i = 0; i < 4; i++)
            {
                angle = q.normalized * angle;
                Shot(false);
            }
        }
        void AttackTypeG() //ランダム　交互
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, Random.Range(40f, 80f), 0f);
            angle = q.normalized * transform.forward;

            //射撃
            for (int i = 0; i < 8; i++)
            {
                Shot(isDefault);
                Destroy(b, 6.0f);
                angle = q.normalized * angle;
            }
            isDefault = !isDefault;
        }
        void AttackTypeH() //追従　特殊弾
        {
            angle = transform.forward;
            Shot(false);
        }
        void AttackTypeI() //3発　特殊弾
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, -15f, 0f);
            angle = q.normalized * transform.forward;
            q = Quaternion.Euler(0f, 15f, 0f);

            //射撃
            for (int i = 0; i < 3; i++)
            {
                Shot(false);
                angle = q.normalized * angle;
            }
        }
        void AttackTypeJ() //ランダム　特殊弾
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, Random.Range(40f, 80f), 0f);
            angle = q.normalized * transform.forward;

            //射撃
            for (int i = 0; i < 8; i++)
            {
                Shot(false);
                angle = q.normalized * angle;
            }
        }
    }

    

    //発射(true:通常弾　false:特殊弾)
    private void Shot(bool tf)
    {
        if (tf)
        {
            b = Instantiate(defaultBullet, transform.position, transform.rotation);
        }
        else
        {
            b = Instantiate(specialBullet, transform.position, transform.rotation);
        }
        rb = b.GetComponent<Rigidbody>();
        rb.AddForce(angle * power);
        Destroy(b, 6.0f);
    }

    //向き追従
    private void LookAtPlayer()
    {
        // 補完スピード
        float speed = 0.1f;
        // ターゲット方向のベクトル
        Vector3 relativePos = player.transform.position - this.transform.position;
        // 方向を、回転情報に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
    }

    //弾が当たった時
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            EnemyDamaged();
        }
    }

    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    private void EnemyDamaged()
    {
        //HPを減らす
        hp--;

        //HitEffect
        SoundManager.instance.PlaySE(SoundManager.SE_Type.S_Damage);
        GameObject spawnedHit = Instantiate(hitEffect, transform.position, transform.rotation, this.transform);
        Destroy(spawnedHit,1.0f);

        //HpDisplay
        switch(hp)
        {
            case 30:
                SwitchColor(secondColor);
                break;
            case 20:
                SwitchColor(thirdColor);
                break;
            case 10:
                SwitchColor(fourthColor);
                break;
            default:
                break;
        }

        //残りHPの色を変える(使用するColor)
        void SwitchColor(Color c) 
        {
            for (int i = hp; i >= 0; i--)
            {
                hpWall[i].GetComponent<MeshRenderer>().material.SetColor("_Color", c);
            }
        }

        //HPを減らす表示
        hpWall[hp].GetComponent<MeshRenderer>().material.SetColor("_Color", damagedColor);

        //HPが0の時終了

        if (hp == 0)
        {
            //shootingの停止
            sm.ShootingEndGame();
            //アニメーション
            StartCoroutine(DestroyAnimation());
        }
    }

    /// <summary>
    /// 破壊アニメーション
    /// </summary>
    private IEnumerator DestroyAnimation()
    {
        completeCanvas.SetActive(true); //クリア画面表示　
        Instantiate(destroyEffect, this.transform.position, this.transform.rotation, this.transform); //エフェクトを生成
        this.GetComponent<MeshRenderer>().enabled = false; //エネミーの見た目を消す
        SoundManager.instance.PlaySE(SoundManager.SE_Type.S_Destroy); //SE

        yield return new WaitForSeconds(0.2f); //クリアアニメーション表示時間待機
        mGameManager.StartCoroutine("UnloadScene", true); //メインゲームマネージャーにクリア判定を送る
    }
}
