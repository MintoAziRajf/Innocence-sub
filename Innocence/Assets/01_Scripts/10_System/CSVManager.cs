using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.AddressableAssets;

public class CSVManager : SingletonMonoBehaviour<CSVManager>
{
    public static CSVManager instance;
    private int stages;
    public int Stages
    {
        get 
        {
            return stages; 
        }

        set 
        { 
            stages = value;
            Debug.Log("ステージが[" + value + "]に変更されました。");
        }
    }

    //Loadしてるかどうか
    private bool isLoad = false;

    //CSV格納場所
    TextAsset masterCSV; //全ステージの情報
    List<string[]> masterDatas = new List<string[]>();//masterCSVのリスト
    List<List<string[]>> mainDatas = new List<List<string[]>>();//各ステージで使用するメインデータ
    public List<string[]> MainDatas
    {
        get
        {
            return mainDatas[stages];
        }
    }
    List<List<string[]>> subDatas = new List<List<string[]>>();//サブデータ
    public List<string[]> SubDatas
    {
        get
        {
            return subDatas[stages];
        }
    }
    List<string[]> skillCheckDatas = new List<string[]>();//ミニゲームデータ(SkillCheckのみ)
    public string[] SkillCheckInfo(int diff)
    {
        return skillCheckDatas[diff];
    }

    void Awake()
    {
        //Singleton
        // シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(LoadAllCSV());
        LoadPlayerData();
    }

    /// <summary>
    /// CSVを読み込む
    /// </summary>
    /// <param name="name">AssetBundleに登録している名前</param>
    /// <param name="datas">読み込んだCSVの保存先</param>
    /// <param name="b">csvの一行目をスキップするか</param>
    private void LoadCSV(string name, List<string[]> datas, bool b)
    {
        //ロード開始
        isLoad = true;
        //Debug.Log(name + ".csvの読み込みを開始します。");

        string path = Path.Combine("Assets/08_CSV/" + name + ".csv");
        Addressables.LoadAssetAsync<TextAsset>(path).Completed += csv =>
        {
            StringReader reader = new StringReader(csv.Result.text);

            string line = null;
            if (b) line = reader.ReadLine(); //bがtrueなら一行目をスキップする
            while (reader.Peek() != -1) // reader.Peekが-1になるまで
            {
                line = reader.ReadLine(); // 一行ずつ読み込み
                datas.Add(line.Split(',')); // , 区切りでリストに追加
            }
            reader.Close();
            //ロード完了
            isLoad = false;
            //Debug.Log(name + ".csvの読み込みを終了します。");
        };
    }

    /// <summary>
    /// 全てのCSVを読み込む
    /// </summary>
    private IEnumerator LoadAllCSV()
    {
        ClearAllDatas();
        // CSVから全ステージの情報をロードする
        LoadCSV("Master", masterDatas, true);
        yield return new WaitUntil(() => !isLoad);

        //各ステージのメインデータとサブデータをロードする
        for (int i = 0; i < masterDatas.Count; i++)
        {
            //メインデータ
            mainDatas.Add(new List<string[]>()); //保存するリストを追加
            LoadCSV(masterDatas[i][2], mainDatas[i], false);
            yield return new WaitUntil(() => !isLoad);
            //サブデータ
            subDatas.Add(new List<string[]>()); //保存するリストを追加
            LoadCSV(masterDatas[i][3], subDatas[i], true);
            yield return new WaitUntil(() => !isLoad);
        }
        //ミニゲームのデータを読み込む
        LoadCSV("SkillCheck", skillCheckDatas, true);
    }

    private void ClearAllDatas()
    {
        //前回の情報を全て削除
        masterDatas.Clear();
        mainDatas.Clear();
        subDatas.Clear();
        skillCheckDatas.Clear();
    }


    //ステージ遷移プレハブ
    [SerializeField] private GameObject loadPrefab = null;
    /// <summary>
    /// stagesから次にロードするステージを判定し、ロードする
    /// </summary>
    public void LoadGame()
    {
        //ステージ遷移オブジェクトを生成
        GameObject SceneLoader = Instantiate(loadPrefab);
        //ステージ遷移スクリプトを取得
        Loading loading = SceneLoader.GetComponent<Loading>();

        if (stages == masterDatas.Count)
        {
            Debug.Log("stagesが" + stages + "なので、エンドシーンをロードします。");
            //ステージ遷移(第二引数にシーン名)
            loading.StartCoroutine("SceneLoading", "05_Ending");
            return;
        }
        else if(stages == -1)
        {
            Debug.Log("stagesが" + stages + "なので、タイトルシーンをロードします。");
            loading.StartCoroutine("SceneLoading", "00_Title");
            return;
        }
        else
        {
            if (masterDatas[stages][1] == "TRUE")
            {
                Debug.Log("stagesが" + stages + "なので、パズルシーンをロードします。");
                loading.StartCoroutine("SceneLoading", "01_MainGame");
                
            }
            else if (masterDatas[stages][1] == "FALSE")
            {
                Debug.Log("stagesが" + stages + "なので、プロローグシーンをロードします。");
                loading.StartCoroutine("SceneLoading", "01_Battle");
                //loading.StartCoroutine("SceneLoading", "Prologue_" + ((stages + 1) / 3).ToString("0"));
            }
        }
    }

    /// <summary>
    /// プロローグかバトルシーンから呼び出される用
    /// </summary>
    /// <param name="isPrologue">プロローグからかどうか</param>
    public void LoadGame(bool isPrologue)
    {
        //ステージ遷移オブジェクトを生成
        GameObject SceneLoader = Instantiate(loadPrefab);
        //ステージ遷移スクリプトを取得
        Loading loading = SceneLoader.GetComponent<Loading>();
        if (isPrologue)
        {
            loading.StartCoroutine("SceneLoading", "01_Battle");
        }
        else
        {
            //Epilogueをロード
            loading.StartCoroutine("SceneLoading", "Epilogue_" + (stages / 3).ToString("0"));
        }
    }

    /// <summary>
    /// プレイヤーデータ関連
    /// </summary>
    private string playerPath = null; //プレイヤーデータの保存先パス
    TextAsset playerCSV; //全ステージのクリア情報
    List<string[]> playerDatas = new List<string[]>();//playerCSVのリスト
    public List<string[]> PlayerDatas
    {
        get { return playerDatas;}
    }


    /// <summary>
    /// プレイヤーデータのロード
    /// プレイヤーデータが存在しない場合、初期化
    /// </summary>
    private void LoadPlayerData()
    {
        playerPath = Application.dataPath + @"\08_CSV\PlayerSave.csv";
        if (!File.Exists(playerPath))
        {
            using (File.Create(playerPath)) ;
            using (StreamWriter streamWriter = new StreamWriter(playerPath, false, Encoding.UTF8))
            {
                for (int i = 0; i < 12; i++)
                {
                    streamWriter.Write("FALSE,C");
                    streamWriter.WriteLine();
                }
                streamWriter.Flush();
                streamWriter.Close();
                Debug.Log("プレイヤーデータの初期化に成功しました。");
            }
        }

        StreamReader csv = new StreamReader(playerPath, Encoding.UTF8);
        string line = null;
        while ((line = csv.ReadLine()) != null)
        {
            playerDatas.Add(line.Split(','));
        }
        csv.Close();
        Debug.Log("プレイヤーデータをロードしました。");
    }

    /// <summary>
    /// クリアデータの保存、その後再読み込み
    /// </summary>
    /// <param name="curretStages">現在のステージ</param>
    /// <param name="remainingSteps">残り歩数</param>
    public void KeepPlayerData(int curretStages, int remainingSteps)
    {
        using (StreamWriter streamWriter = new StreamWriter(playerPath, false, Encoding.UTF8))
        {
            for (int i = 0; i < playerDatas.Count; i++)
            {
                if (i == curretStages)
                {
                    string s;
                    if (remainingSteps >= 10) s = "S";
                    else if (remainingSteps >= 5) s = "A";
                    else s = "B";
                    streamWriter.WriteLine("TRUE," + s);
                }
                else
                {
                    streamWriter.WriteLine(playerDatas[i][0] + "," + playerDatas[i][1]);
                }
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        //再読み込み
        LoadPlayerData();
    }

    /// <summary>
    /// -------------------------------------------------Debug-------------------------------------------------
    /// </summary>
    /// <param name="num">強制的にステージを変更する</param>
    public void LoadGame(int num)
    {
        this.Stages = num;
        LoadGame();
    }

    [SerializeField] private int debugStage = 0;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.P))
        {
            this.Stages = debugStage;
            LoadGame(this.Stages);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O))
        {
            LoadPlayerData();
        }
    }
    
}
