using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;//DebugLog用

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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadMasterDate();
        LoadPlayerData();
        LoadAllCSV();
    }

    /// <summary>
    /// CSVから全ステージの情報をロードする
    /// </summary>
    private void LoadMasterDate()
    {
        masterDatas.Clear(); //前回の情報を全て削除
        string path = Application.dataPath + @"/08_CSV/Master.csv";
        StreamReader csv = new StreamReader(path, Encoding.UTF8); //StreamReaderにCSV読み込む
        string line = csv.ReadLine(); //見出し行をスキップさせる
        while ((line = csv.ReadLine()) != null) // , で分割しつつ一行ずつ読み込み
        {
            masterDatas.Add(line.Split(','));
        }
    }

    /// <summary>
    /// 全てのCSVを読み込む
    /// </summary>
    private void LoadAllCSV()
    {
        //前回の情報を全て削除
        mainDatas.Clear();
        subDatas.Clear();
        //CSVをひとつづつListに追加する
        for (int i = 0; i < masterDatas.Count; i++)
        {
            //メインデータ
            mainDatas.Add(new List<string[]>()); //保存するリストを追加
            string path = Application.dataPath + @"/08_CSV/" + masterDatas[i][2] + ".csv"; //Masterで指定されているCSVを読み込む
            StreamReader csv = new StreamReader(path, Encoding.UTF8); //StreamReaderにCSV読み込む

            string line = null; 
            while ((line = csv.ReadLine()) != null) // , で分割しつつ一行ずつ読み込み
            {
                mainDatas[i].Add(line.Split(','));
            }
            csv.Close();

            //サブデータ
            subDatas.Add(new List<string[]>()); //保存するリストを追加
            path = Application.dataPath + @"/08_CSV/" + masterDatas[i][3] + ".csv"; //Masterで指定されているCSVを読み込む
            csv = new StreamReader(path, Encoding.UTF8); //StreamReaderにCSV読み込む
            line = csv.ReadLine(); //見出し行をスキップさせる
            while ((line = csv.ReadLine()) != null) // , で分割しつつ一行ずつ読み込み
            {
                subDatas[i].Add(line.Split(','));
            }
            csv.Close();
        }
        Debug.Log("全てのCSVを読み込みました。");
    }

    //ステージ遷移プレハブ
    [SerializeField] private GameObject loadPrefab = null;
    /// <summary>
    /// stagesから次にロードするステージを判定し、ロードする
    /// </summary>
    public void LoadGame()
    {
        if (masterDatas[stages][1] == "TRUE")
        {
            Debug.Log("stagesが" + stages + "なので、バトルシーンをロードします。");
            //ステージ遷移オブジェクトを生成
            GameObject SceneLoader = Instantiate(loadPrefab);
            //ステージ遷移スクリプトを取得
            Loading loading = SceneLoader.GetComponent<Loading>();
            //ステージ遷移(第二引数にシーン名)
            loading.StartCoroutine("SceneLoading", "01_Battle");
        }
        else if(masterDatas[stages][1] == "FALSE")
        {
            Debug.Log("stagesが" + stages + "なので、パズルシーンをロードします。");
            GameObject SceneLoader = Instantiate(loadPrefab);
            Loading loading = SceneLoader.GetComponent<Loading>();
            loading.StartCoroutine("SceneLoading", "01_MainGame");
        }
        else
        {
            Debug.Log("stagesが" + stages + "なので、エンドシーンをロードします。");
            GameObject SceneLoader = Instantiate(loadPrefab);
            Loading loading = SceneLoader.GetComponent<Loading>();
            loading.StartCoroutine("SceneLoading", "05_Ending");
        }
    }


    /// <summary>
    /// プレイヤーデータ関連
    /// </summary>
    TextAsset playerCSV; //全ステージのクリア情報
    List<string[]> playerDatas = new List<string[]>();//playerCSVのリスト
    public List<string[]> PlayerDatas
    {
        get { return playerDatas; }
    }
    /// <summary>
    /// プレイヤーデータのロード
    /// </summary>
    private void LoadPlayerData()
    {
        playerDatas.Clear();
        string path = Application.dataPath + @"/08_CSV/PlayerSave.csv";
        StreamReader csv = new StreamReader(path, Encoding.UTF8);
        string line = null;
        while ((line = csv.ReadLine()) != null)
        {
            playerDatas.Add(line.Split(','));
        }
        csv.Close();
    }

    /// <summary>
    /// クリアデータの保存
    /// </summary>
    /// <param name="curretStages">現在のステージ</param>
    /// <param name="remainingSteps">残り歩数</param>
    public void KeepPlayerData(int curretStages, int remainingSteps)
    {
        string path = Application.dataPath + @"/08_CSV/PlayerSave.csv";

        using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
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
    }

    /// <summary>
    /// プレイヤーデータの初期化(Debug用)
    /// </summary>
    private void InitializePlayerData()
    {
        LoadPlayerData();
        string path = Application.dataPath + @"/08_CSV/PlayerSave.csv";
        using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
        {
            Debug.Log(playerDatas.Count);
            for (int i = 0; i < playerDatas.Count; i++)
            {
                streamWriter.Write("FALSE,C");
                streamWriter.WriteLine();
            }
            streamWriter.Flush();
            streamWriter.Close();
        }
    }


    /// <summary>
    /// Debug用
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
            InitializePlayerData();
        }
    }
    
}
