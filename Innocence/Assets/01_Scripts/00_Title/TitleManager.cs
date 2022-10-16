using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    //animator
    Animator animator;
    //タイトルかステージセレクトか
    private bool isTitle = true;

    //EventSystem
    private EventSystem eventSystem = null;
    private bool isFirst = true;
    //ステージ遷移プレハブ
    [SerializeField] private GameObject loadPrefab = null;

    //csvManager
    CSVManager csvManager;

    void Start()
    {
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Title);
        animator = GetComponent<Animator>();
        animator.SetBool("isTitle", true);
    }

    void Update()
    {
        if (isTitle) return;　//タイトル画面の時は何もしない
        if (Input.GetButtonDown("Cancel"))
        {
            //SoundManager.instance.PlaySE(SoundManager.SE_Type.Cancel);
            isTitle = true;
            animator.SetBool("isTitle", isTitle);
        }
    }

    //決定音
    private void SelectSubmit()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
    }

    //ステージ 1
    public void PushNewGame()
    {
        if (!isFirst) return; //一度目のみ動作させる
        isFirst = false; 
        SelectSubmit();  //SE
        StartCoroutine(LoadNewGame());
    }
    private IEnumerator LoadNewGame()
    {
        animator.SetTrigger("selectNewGame"); //アニメーション起動
        yield return new WaitForSeconds(0.5f);//アニメーション表示時間待機(0.5秒)
        LoadScene(0); //1ステージ目をロードする
    }

    //ステージセレクト
    public void PushStageSelect()
    {
        SelectSubmit(); //SE
        isTitle = false;
        animator.SetBool("isTitle", isTitle);
    }
    //ステージ 選択
    public void PushStage(int num)
    {
        //animator.SetTrigger("selectedStage");
        SelectSubmit();
        LoadScene(num);
    }

    //シーン遷移
    private void LoadScene(int num)
    {
        csvManager.Stages = num; //ステージ数をCSVManagerに送る
        csvManager.LoadGame();　//指定したステージをロードする
    }

    //クレジットシーン表示
    public void PushCredit()
    {
        if (!isFirst) return;
        isFirst = false;
        SelectSubmit();
        StartCoroutine(LoadCredit());
    }
    private IEnumerator LoadCredit()
    {
        animator.SetTrigger("selectCredit"); //アニメーション起動
        yield return new WaitForSeconds(0.5f);//アニメーション表示時間待機(0.5秒)
        //クレジットシーンをロード
        GameObject SceneLoader = Instantiate(loadPrefab);
        Loading loading = SceneLoader.GetComponent<Loading>();
        loading.StartCoroutine("SceneLoading", "06_Credits");
    }

    //ゲーム終了
    public void ExitGame() => Application.Quit();
}
