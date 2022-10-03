using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameManager : SingletonMonoBehaviour<MainGameManager>
{
    //フラグ
    private bool isStart = false; //スタートしているかどうか
    private bool IsStart
    {
        set
        {
            isStart = value;
            playerController.IsStart = value;
        }
    }
    private bool isClear = false; //ミニゲームの成否
    [SerializeField] private bool isBattle = false; //バトルシーンかどうか

    //ミニゲーム難易度受け渡し
    private int difficulty;
    public int Difficulty
    {
        get { return this.difficulty; }
        set { this.difficulty = value; }
    }

    //追加したシーンの名前を保存
    private string sceneName = "";

    //トランジション終了フラグ
    private bool isCompleted = false;

    //transition
    private GameObject transitionObj = null;
    MiniGameTransition transition;

    

    //object Controller
    private PlayerController playerController;
    private GameObject player;
    private StoneController stoneController;

    //Pause
    [SerializeField] private GameObject pauseObj = null;
    //Scene遷移
    [SerializeField] private GameObject loadPrefab = null;

    //csvManager
    CSVManager csvManager;

    //ミニゲーム遷移時にメインゲームを非アクティブにする用
    private GameObject stage;

    //gameover,clear
    [SerializeField] private GameObject gameoverObj = null;
    [SerializeField] private GameObject gameclearObj = null;

    private void Awake()
    {
        //Init
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();
        transitionObj = GameObject.Find("Transition");
        transition = transitionObj.GetComponent<MiniGameTransition>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        stage = GameObject.Find("Grid");
        
        StartCoroutine(StartGame());

        //ゲーム開始一秒間入力を受けつけない
        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(1.0f);
            this.IsStart = true;
        }
    }

    private void Update()
    {
        if (!isStart) return;
        //リスタート
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadStage();
            return;
        }
        if (Input.GetButtonDown("Option"))
        {
            Pause();
            return;
        }
    }

    //ゲームオーバー
    public void GameOver()
    {
        //全ての操作を停止
        this.IsStart = false;
        player.SetActive(false);
        gameoverObj.SetActive(true);
        //Battleステージの場合
        if(isBattle)
        {
            GameObject.Find("BossController").GetComponent<BossController>().EndBattle();
        }
        SoundManager.instance.PlaySE(SoundManager.SE_Type.GameOver);
        StartCoroutine(LoadStageDelay());
        //Delay
        IEnumerator LoadStageDelay()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            //リスタート
            LoadStage();
        }
    }

    //次のステージへ遷移
    public IEnumerator NextStage()
    {
        //全ての操作を停止
        this.IsStart = false;
        yield return new WaitForSeconds(1.0f);//delay
        gameclearObj.SetActive(true); //ゲームクリア画面表示
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.M_Goal);
        //リザルトスクリプトに後の処理を渡す
        gameclearObj.GetComponent<Result>().ResultData(playerController.Steps);
        //クリアデータの保存
        csvManager.KeepPlayerData(csvManager.Stages, playerController.Steps);
        //CSVManagerに次のステージ数を渡す
        csvManager.Stages = csvManager.Stages + 1;
        yield return new WaitForSeconds(3.0f); //result表示中待機
        //yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space)); //待機後Spaceキーで次のステージへ
        csvManager.LoadGame();
    }

    //シーンのロード
    private void LoadStage()
    {
        //全ての操作を停止
        this.IsStart = false;

        csvManager.LoadGame();
    }

    //ポーズ
    private void Pause() 
    {
        //ポーズ
        this.IsStart = false;
        //OptionMenuのアクティブ
        pauseObj.SetActive(true);
        //時間を止める
        Time.timeScale = 0f;
    }
    //ゲームを再開する
    public void Resume()
    {
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
        //再開
        this.IsStart = true;
        //OptionMenuの非アクティブ
        pauseObj.SetActive(false);
        //時間を進める
        Time.timeScale = 1f;
    }
    //Titleに戻る
    public void OnClickMainMenu()
    {
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
        //SceneLoad
        GameObject SceneLoader = Instantiate(loadPrefab);
        Loading loading = SceneLoader.GetComponent<Loading>();
        loading.StartCoroutine("SceneLoading", "00_Title");
    }
    //ミニゲームをすべてスキップする
    public void　SkipMiniGame()
    {
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
        //配列
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

        foreach (GameObject stone in stones)
        {
            //すべてのstoneにアクセスできるようにする
            stone.GetComponent<StoneController>().IsAccess = true;
        }
        Resume();
    }
    //画面遷移終了判定
    public void CompletedTransition()
    {
        isCompleted = true;
    }
    //ミニゲームシーンの追加
    public IEnumerator SceneAdd(string sceneName)
    {
        //全ての操作の停止
        this.IsStart = false;
        transition.StartCoroutine("StartTransition");
        this.sceneName = sceneName;
        // 非同期でロードを行う
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(this.sceneName, LoadSceneMode.Additive);

        // ロードが完了していても，シーンのアクティブ化は許可しない
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) //0.9で止まる
        {
            yield return 0;
        }

        while (isCompleted == false) //トランジション終了まで待機
        {
            yield return 0;
        }

        // ロードが完了したときにシーンのアクティブ化を許可する
        asyncLoad.allowSceneActivation = true;

        //メインゲームを非アクティブにする
        stage.SetActive(false);

        // ロードが完了するまで待つ
        yield return asyncLoad;
        //トランジション終了フラグの初期化
        isCompleted = false;
        //ミニゲームクリアフラグの初期化
        isClear = false;
        Debug.Log(sceneName);
    }
    //ミニゲームシーンのアンロード
    public IEnumerator UnloadScene(bool b)
    {
        isClear = b;
        transition.StartCoroutine("StartTransition");
        while (!isCompleted) //トランジション終了まで待機
        {
            yield return 0;
        }
        //メインゲームをアクティブ
        stage.SetActive(true);
        yield return null;
        if (isClear)
        {
            stoneController.IsAccess = true;
        }
        else
        {
            stoneController.IsAccess = false;
        }
        //ミニゲームシーンの削除
        SceneManager.UnloadSceneAsync(this.sceneName);
        //トランジション終了フラグの初期化
        isCompleted = false;
        yield return new WaitForSeconds(0.5f);
        //ゲーム再開
        this.IsStart = true;
    }

    public void SetStone(StoneController nowControll)
    {
        stoneController = nowControll;
    }
}
