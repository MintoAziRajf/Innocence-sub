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

    //ステージセレクト用
    //EventSystem
    [SerializeField] private EventSystem eventSystem = null;
    //選択中のオブジェクト
    private GameObject selectedButton;
    //ステージ情報
    StageInfo stageInfo;

    //ステージ遷移プレハブ
    [SerializeField] private GameObject loadPrefab = null;

    //csvManager
    CSVManager csvManager;

    void Start()
    {
        stageInfo = GetComponent<StageInfo>();
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Title);
        animator = GetComponent<Animator>();
        animator.SetBool("isTitle", true);
        animator.SetInteger("stageNum", 0);
    }

    void Update()
    {
        if (isTitle) return;
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
        SelectSubmit();
        animator.SetTrigger("selectNewGame");
        LoadScene(0);
    }
    //ステージセレクト
    public void PushStageSelect()
    {
        SelectSubmit();
        isTitle = false;
        animator.SetBool("isTitle", isTitle);
    }
    //ステージ 選択
    public void PushStage(int num)
    {
        animator.SetTrigger("selectedStage");
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
        SelectSubmit();
        LoadScene(num);
    }

    //シーン遷移
    private void LoadScene(int num)
    {
        csvManager.Stages = num;
        csvManager.LoadGame();
    }

    //クレジットシーン表示
    public void PushCredit()
    {
        SelectSubmit();
        StartCoroutine(LoadCredit());
    }
    private IEnumerator LoadCredit()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject SceneLoader = Instantiate(loadPrefab);
        Loading loading = SceneLoader.GetComponent<Loading>();
        loading.StartCoroutine("SceneLoading", "06_Credits");
    }

    //ゲーム終了
    public void ExitGame()
    {
        Application.Quit();
    }
}
