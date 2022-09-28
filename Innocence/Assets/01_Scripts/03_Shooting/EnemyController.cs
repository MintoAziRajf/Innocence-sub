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

    //攻撃タイプ
    [SerializeField] private int type = 0;

    //ステータス
    private int hp = 40;
    //hp表示用
    [SerializeField] private GameObject[] hpWall = null;
    [SerializeField] private Material mat = null;

    //射撃
    private float interval = 0.2f;
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
        targetPos = this.transform.position;
    }

    void FixedUpdate()
    {
        if (!isStart) return;
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
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.0f * Time.deltaTime);
            //目的地についていなければ何もしない
            if (!(this.transform.position == targetPos)) return;

            //目的地をセット
            targetPos.x = Random.Range(-4.0f, 4.0f);
            targetPos.z = Random.Range(-4.0f, 4.0f);
        }

        void AttackTypeA() //0:追従　通常弾
        {
            angle = transform.forward;
            Shot(true);
        }
        void AttackTypeB() //1:追従　特殊弾
        {
            angle = transform.forward;
            Shot(false);
        }
        void AttackTypeC() //2:追従　弾交互
        {
            angle = transform.forward;
            //弾の生成・発射
            Shot(isDefault);

            //弾切り替え
            isDefault = !isDefault;
        }
        void AttackTypeD() //3:回転　弾種交互
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, 45f, 0f);
            Quaternion q2 = Quaternion.Euler(0f, 3f, 0f);
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
        void AttackTypeE() //4:回転　特殊弾のみ
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
        void AttackTypeF() //5:トリニティ　通常弾
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, -45f, 0f);
            angle = q.normalized * transform.forward;
            q = Quaternion.Euler(0f, 45f, 0f);

            //射撃
            for (int i = 0; i < 3; i++)
            {
                Shot(true);
                angle = q.normalized * angle;
            }
        }
        void AttackTypeG() //6:トリニティ　特殊弾
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, -45f, 0f);
            angle = q.normalized * transform.forward;
            q = Quaternion.Euler(0f, 45f, 0f);

            //射撃
            for (int i = 0; i < 3; i++)
            {
                Shot(false);
                angle = q.normalized * angle;
            }
        }
        void AttackTypeH() //7:トリニティ　交互
        {
            //角度
            Quaternion q = Quaternion.Euler(0f, -45f, 0f);
            angle = q.normalized * transform.forward;
            q = Quaternion.Euler(0f, 45f, 0f);

            //射撃
            for (int i = 0; i < 3; i++)
            {
                Shot(isDefault);
                angle = q.normalized * angle;
            }
            isDefault = !isDefault;
        }
        void AttackTypeI() //8:ランダム　特殊弾
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
        void AttackTypeJ() //9:ランダム　交互
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            EnemyDamaged();
        }
    }

    private void EnemyDamaged()
    {
        hp--;

        //HitEffect
        SoundManager.instance.PlaySE(SoundManager.SE_Type.S_Damage);
        GameObject spawnedHit = Instantiate(hitEffect, transform.position, transform.rotation, this.transform);
        Destroy(spawnedHit,1.0f);

        //HpDisplay

        hpWall[hp].GetComponent<Renderer>().material = mat;

        if (hp == 0)
        {
            //shootingの停止
            sm.ShootingEndGame();
            //アニメーション
            StartCoroutine(DestroyAnimation());
        }
    }

    private IEnumerator DestroyAnimation()
    {
        //animation
        completeCanvas.SetActive(true);
        Instantiate(destroyEffect, this.transform.position, this.transform.rotation, this.transform);
        this.GetComponent<MeshRenderer>().enabled = false;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.S_Destroy);

        yield return new WaitForSeconds(0.2f);
        mGameManager.StartCoroutine("UnloadScene", true);
    }
}
