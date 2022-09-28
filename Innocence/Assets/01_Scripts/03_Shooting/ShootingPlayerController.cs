using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayerController : MonoBehaviour
{
    //移動
    private float h, v;
    private Vector3 moving, prevPos, playerPos;
    [SerializeField] private float speed = 0f;

    //移動できる範囲
    [SerializeField] private float ereaClampX = 0f, ereaClampZ = 0f;

    //Rigidbody
    private Rigidbody rb;

    //敵との距離
    private float range = 10000f;

    //射撃
    [SerializeField] private GameObject bullet = null; //弾prefab
    [SerializeField] private GameObject barrel = null; //発射位置
    [SerializeField] private float power = 0f;　//速さ
    [SerializeField] private float interval = 0f;　//間隔
    private float time = 0f;

    //GameManager
    MainGameManager mGameManager;

    //ShootingController
    ShootingManager sm;

    //フラグ
    private bool isStart = false;
    public bool IsStart
    {
        get { return this.isStart; }
        set { this.isStart = value; }
    }
    private bool isEnemy = false;

    //GameOverEffect
    [SerializeField] private GameObject destroyEffect = null;

    void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();
        sm = GameObject.Find("ShootingManager").GetComponent<ShootingManager>();
    }

    void Update()
    {
        if (!isStart) return;
        //移動入力
        MovementControll();
        Movement();
        //射撃
        if (!isEnemy) return;
        AutoAttack();
    }

    void FixedUpdate()
    {
        RotateDirection();
    }

    //敵の方を向く
    private void RotateDirection()
    {
        //一番近いEnemyタグがついた敵の方を向く
        GameObject nearestEnemy = null;
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            float dis = Vector3.Distance(transform.position, enemy.transform.position);
            if (dis <= range)
            {
                range = dis;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            isEnemy = true;
            float speed = 0.4f;
            Vector3 relativePos = nearestEnemy.transform.position - this.transform.position;
            // 方向を、回転情報に変換
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            // 現在の回転情報と、ターゲット方向の回転情報を補完する
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);

        }
        else
        {
            range = 10000f;
            return;
        }
    }

    //移動速度の正規化
    private void MovementControll()
    {
        //斜め移動と縦横の移動を同じ速度にするために正規化
        moving = new Vector3(h, 0, v);
        moving.Normalize();
        moving = moving * speed;
    }

    //移動
    private void Movement()
    {
        //移動
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        transform.position += (Vector3.right * h + Vector3.forward * v) * speed * Time.deltaTime;

        //移動制限
        playerPos = transform.position;
        this.playerPos = transform.position;
        this.playerPos.x = Mathf.Clamp(this.playerPos.x, -ereaClampX, ereaClampX);
        this.playerPos.z = Mathf.Clamp(this.playerPos.z, -ereaClampZ, ereaClampZ);
        transform.position = new Vector3(this.playerPos.x, this.playerPos.y, this.playerPos.z);
    }

    //自動攻撃
    private void AutoAttack()
    {
        time += Time.deltaTime;
        if (time > interval)
        {
            time = 0f;
            SoundManager.instance.PlaySE(SoundManager.SE_Type.Bullet);
            GameObject b = Instantiate(bullet, barrel.transform.position, transform.rotation);
            Rigidbody rb = b.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * power);
            Destroy(b, 1.0f);
        }
        else
        {
            return;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!isStart) return;
        if (collider.CompareTag("EnemyBulletA"))
        {
            GameOver();
        }
        if (collider.CompareTag("EnemyBulletB"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //Effect
        Instantiate(destroyEffect, this.transform.position, this.transform.rotation, this.transform);
        this.GetComponent<MeshRenderer>().enabled = false;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.S_Destroy);
        //minigameの停止
        sm.ShootingEndGame();
        //mainGameManagerに失敗判定を送る
        mGameManager.StartCoroutine("UnloadScene", false);
    }
}
