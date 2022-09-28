using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartAnimation : MonoBehaviour
{
    //Stage
    [SerializeField] private GameObject[] stage = null;
    private Vector3 stageMove = new Vector3(0f, 10f, 0f);

    //player, boss
    [SerializeField] private SpriteRenderer player = null;
    [SerializeField] private SpriteRenderer boss = null;

    //Novel
    [SerializeField] private GameObject novel = null;

    private void Awake()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Battle);
        
        player.enabled = false;
        boss.enabled = false;
    }

    private void Start()
    {
        Time.timeScale = 0f;
        //Stageの起動
        StartCoroutine(StageMove());
    }

    private IEnumerator Novel()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        //Novel中にプレイヤーとボスをアクティブ
        player.enabled = true;
        boss.enabled = true;
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
        novel.SetActive(false);
        Time.timeScale = 1f;
        GetComponent<BossController>().StartCoroutine("BattleStart");
    }

    private IEnumerator StageMove()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                yield return null;
                stage[i].transform.position = Vector3.MoveTowards(stage[i].transform.position, stage[i].transform.position + stageMove, 10.0f / 15.0f);
            }
            //SE
            yield return new WaitForSecondsRealtime(0.1f);
        }
        //Novel
        novel.SetActive(true);
        StartCoroutine(Novel());
    }
}
