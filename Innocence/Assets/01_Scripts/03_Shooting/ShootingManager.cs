using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    ShootingPlayerController spc;
    EnemyController ec;

    //操作説明オブジェクト
    [SerializeField] private GameObject guide = null;

    void Start()
    {
        spc = GameObject.Find("Player").GetComponent<ShootingPlayerController>();
        ec = GameObject.Find("Enemy").GetComponent<EnemyController>();
        StartCoroutine(StartDelay());
    }

    //ミニゲーム開始までのdelay
    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.2f);//0.2秒表示するまでのdelay
        guide.SetActive(true);//操作説明を表示
        yield return new WaitForSeconds(1.5f);//3.5秒表示後ゲームをスタート
        ShootingStartGame();
    }

    public void ShootingEndGame()
    {
        //弾の削除
        BulletsDestroy();
        //PlayerとEnemyの停止
        spc.IsStart = false;
        ec.IsStart = false;
    }

    private void ShootingStartGame()
    {
        //PlayerとEnemyの進行
        spc.IsStart = true;
        ec.IsStart = true;
    }

    //弾の全削除
    private void BulletsDestroy()
    {
        GameObject[] enemyBulletA = GameObject.FindGameObjectsWithTag("EnemyBulletA");
        foreach (GameObject b in enemyBulletA)
        {
            Destroy(b);
        }
        GameObject[] enemyBulletB = GameObject.FindGameObjectsWithTag("EnemyBulletB");
        foreach (GameObject b in enemyBulletB)
        {
            Destroy(b);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject b in bullets)
        {
            Destroy(b);
        }
    }
}
