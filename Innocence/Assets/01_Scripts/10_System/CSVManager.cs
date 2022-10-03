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
            return mainDatas[stages - 1];
        }
    }
    List<List<string[]>> subDatas = new List<List<string[]>>();//サブデータ
    public List<string[]> SubDatas
    {
        get
        {
            return subDatas[stages - 1];
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadMasterDate();
        LoadAllCSV();
    }

    /// <summary>
    /// CSVから全ステージの情報をロードする
    /// </summary>
    private void LoadMasterDate()
    {
        masterCSV = Resources.Load("CSV/Master") as TextAsset;
        StringReader reader = new StringReader(masterCSV.text);

        // , で分割しつつ一行ずつ読み込み
        //リストに追加
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            masterDatas.Add(line.Split(','));
        }
    }

    /// <summary>
    /// 全てのCSVを読み込む
    /// </summary>
    private void LoadAllCSV()
    {
        TextAsset currentCSV; //CSVを一時保存する
        StringReader reader;
        
        //CSVをひとつづつListに追加する
        for (int i = 1; i < masterDatas.Count; i++)
        {
            currentCSV = Resources.Load("CSV/" + masterDatas[i][2]) as TextAsset; //Masterで指定されているCSVを読み込む
            reader = new StringReader(currentCSV.text);　//StringReaderにCSVをテキストとして読み込む
            mainDatas.Add(new List<string[]>()); //保存するリストを追加
            // , で分割しつつ一行ずつ読み込み
            //リストに追加
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                mainDatas[i - 1].Add(line.Split(','));
            }

            //メインデータと同じ手順でサブデータを読み込む
            currentCSV = Resources.Load("CSV/" + masterDatas[i][3]) as TextAsset;
            reader = new StringReader(currentCSV.text);
            subDatas.Add(new List<string[]>());
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                subDatas[i - 1].Add(line.Split(','));
            }
        }
        Debug.Log("全てのCSVを読み込みました。");
    }

    //ステージ遷移プレハブ
    [SerializeField] private GameObject loadPrefab = null;
    public void LoadGame()
    {
        if (masterDatas[stages][1] == "TRUE")
        {
            Debug.Log("stagesが" + stages + "なので、バトルシーンをロードします。");
            GameObject SceneLoader = Instantiate(loadPrefab);
            Loading loading = SceneLoader.GetComponent<Loading>();
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


    TextAsset playerCSV; //全ステージのクリア情報
    List<string[]> playerDatas = new List<string[]>();//playerCSVのリスト
    /// <summary>
    /// プレイヤーデータのロード(ステージ選択画面で使用)
    /// </summary>
    public void LoadPlayerData()
    {
        playerCSV = Resources.Load("CSV/PlayerSave") as TextAsset;
        StringReader reader = new StringReader(playerCSV.text);

        // , で分割しつつ一行ずつ読み込み
        //リストに追加
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            playerDatas.Add(line.Split(','));
        }
    }

    /// <summary>
    /// クリアデータの保存
    /// </summary>
    /// <param name="curretStages">現在のステージ</param>
    /// <param name="remainingSteps">残り歩数</param>
    public void KeepPlayerData(int curretStages, int remainingSteps)
    {
        LoadPlayerData();
        StreamWriter sw;
        FileInfo fi;
        string path = Application.dataPath + "/Resources/CSV/PlayerSave.csv";
        fi = new FileInfo(path);
        sw = fi.CreateText();
        for(int i = 0; i < playerDatas.Count; i++)
        {
            if(i == curretStages - 1)
            {
                string s;
                if(remainingSteps >= 10) s = "S";
                else if(remainingSteps >= 5) s = "A";
                else s = "B";
                sw.WriteLine("TRUE," + s);
            }
            else
            {
                sw.WriteLine(playerDatas[i][0] + "," + playerDatas[i][1]);
            }
        }
        sw.Flush();
        sw.Close();
        Debug.Log("クリアデータを保存しました。");
    }

    private void InitializePlayerData()
    {
        LoadPlayerData();
        StreamWriter sw;
        FileInfo fi;
        string path = Application.dataPath + "/Resources/CSV/PlayerSave.csv";
        string s = "FALSE,C";
        fi = new FileInfo(path);
        sw = fi.CreateText();
        Debug.Log(playerDatas.Count);
        for (int i = 0; i < playerDatas.Count; i++)
        {
            sw.WriteLine("FALSE" + "," + "C");
        }
        sw.Flush();
        sw.Close();
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
    [SerializeField] Text text = null;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
            text.text = playerDatas[0][0] + "," + playerDatas[0][1];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.P))
        {
            this.Stages = 1;
            LoadGame(this.Stages);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O))
        {
            InitializePlayerData();
        }
    }
    
}
