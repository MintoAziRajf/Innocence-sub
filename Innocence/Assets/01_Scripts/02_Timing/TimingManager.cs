using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingManager : MonoBehaviour
{
    //mainGameManager
    MainGameManager mainGameManager;

    //map
    [SerializeField] private GameObject[] map = null;
    [SerializeField] private int mapNum = 0;

    void Start()
    {
        //mapの生成
        mainGameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();
        mapNum = mainGameManager.Difficulty;
        map[mapNum].SetActive(true);
    }

    //ミニゲーム終了
    IEnumerator EndGame(bool tf)
    {
        yield return new WaitForSeconds(0.5f);
        //シーンのアンロード,ゲームマネージャーに成否を送る
        mainGameManager.StartCoroutine("UnloadScene", tf);
    }
}
