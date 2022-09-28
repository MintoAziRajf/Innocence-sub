using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    ShootingPlayerController spc;
    EnemyController ec;
    //説明
    [SerializeField] private GameObject guide = null;

    void Start()
    {
        spc = GameObject.Find("Player").GetComponent<ShootingPlayerController>();
        ec = GameObject.Find("Enemy").GetComponent<EnemyController>();
        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.2f);
        guide.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        ShootingStartGame();
    }

    public void ShootingEndGame()
    {
        BulletsDestroy();
        //PlayerとEnemyの停止
        spc.IsStart = false;
        ec.IsStart = false;
    }

    private void ShootingStartGame()
    {
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
