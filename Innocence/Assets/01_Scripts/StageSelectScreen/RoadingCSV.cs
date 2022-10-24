using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using StageSelectScene;
using UnityEngine.UI;

public class RoadingCSV : MonoBehaviour
{
    //ファイルのパス
    [SerializeField] string csvPath = null;

    [SerializeField] StageSelectionScreen stage = null;
    //csvファイル読み込み用
    List<string[]> csvDatas = new List<string[]>();
    List<string[]> playerDatas = new List<string[]>();

    [SerializeField]
    Text[] stageTitles = new Text[13];

    Text[] staminas = new Text[13];
    [SerializeField] Text stamina = null;
    [SerializeField]
    Score[] clearEvaluations = new Score[13];

    bool loadingCheckCsv = false;
    bool loadingCheckPlayer = false;

    public Score[] ClearEvaluations => clearEvaluations;
    public Text[] Staminas => staminas;
    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(Roadingcsv());        
    }

    IEnumerator Roadingcsv()
    {
       
        enabled = false;

        Addressables.LoadAssetAsync<TextAsset>(csvPath).Completed += CSVData =>
        {
            StringReader readerCsv = new StringReader(CSVData.Result.text);

            while (readerCsv.Peek() != -1)
            {
                string line = readerCsv.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            enabled = true;
            loadingCheckCsv = true;
        };



        /*
        Addressables.LoadAssetAsync<TextAsset>(playerSave).Completed += playerData =>
        {
            StringReader readerPlayer = new StringReader(playerData.Result.text);

            while (readerPlayer.Peek() != -1)
            {
                string line = readerPlayer.ReadLine();
                playerDatas.Add(line.Split(','));
            }
            enabled = true;
            loadingCheckPlayer = true;
        };

        yield return new WaitUntil(() => loadingCheckCsv && loadingCheckPlayer);
        */

        //--------------追加---------------
        playerDatas = CSVManager.instance.PlayerDatas;
        loadingCheckPlayer = true;
        //---------------------------------

        yield return new WaitUntil(() => loadingCheckCsv && loadingCheckPlayer);

        //playersaveの二列目取得
        for (int i = 1; i < 12; ++i)
        {
            if (playerDatas[i - 1][1] == "S") clearEvaluations[i] = Score.SCORE3;
            else if (playerDatas[i - 1][1] == "A") clearEvaluations[i] = Score.SCORE2;
            else if (playerDatas[i - 1][1] == "B") clearEvaluations[i] = Score.SCORE1;
            else if (playerDatas[i - 1][1] == "C") clearEvaluations[i] = Score.SCORE0;
            else clearEvaluations[i - 1] = Score.SCORE0;

           
        }

    }
    // Update is called once per frame
    void Update()
    {
        //一列目にステージのタイトル
        //二列目にステージのスタミナ

        for (int i = 0; i < 13; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {

                if (j == 0) { stageTitles[i].text = csvDatas[i][j]; }
                if (j == 1 &&  "Stage" + i == stage.Names) { stamina.text = csvDatas[i][j]; }
            }
        }
    }

      
}
