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
    [SerializeField] string csvPath,playerSave;
    

    //csvファイル読み込み用
    List<string[]> csvDatas = new List<string[]>();
    List<string[]> playerDatas = new List<string[]>();

    [SerializeField]
    Text[] StageTitles = new Text[13];
    [SerializeField]
    Text[] Staminas = new Text[13];
    [SerializeField]
    Score[] ClearEvaluations = new Score[13];

    public Score[] clearEvaluations => ClearEvaluations;
    // Start is called before the first frame update
    void Start()
    {



      

        StartCoroutine(Roadingcsv());

        //一列目にステージのタイトル
        //二列目にステージのスタミナ
        //三列目にステージのクリア評価


        /*for (int i = 0; i < 13; ++i)
        {
            for (int j = 0; j < 2; ++i)
            {
                if (i == 0) { StageTitles[i].text = csvDatas[i][j]; }
                if (j == 1) { Staminas[i].text = csvDatas[i][j]; }
            }
        }

        for (int i = 0; i < 11; ++i)
        {
            if (playerDatas[i][1] == "S") ClearEvaluations[i] = Score.SCORE3;
            if (playerDatas[i][1] == "A") ClearEvaluations[i] = Score.SCORE2;
            if (playerDatas[i][1] == "B") ClearEvaluations[i] = Score.SCORE1;
            if (playerDatas[i][1] == "C") ClearEvaluations[i] = Score.SCORE0;

            Debug.Log(ClearEvaluations[i]);
        }*/

    }

    IEnumerator Roadingcsv()
    {
       
        enabled = false;

        Addressables.LoadAssetAsync<TextAsset>(csvPath).Completed += CSVData =>
        {
            StringReader reader = new StringReader(CSVData.Result.text);

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            enabled = true;
        };

        
       

        Addressables.LoadAssetAsync<TextAsset>(playerSave).Completed += playerData =>
        {
            StringReader reader = new StringReader(playerData.Result.text);

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                playerDatas.Add(line.Split(','));
            }
            enabled = true;
        };

        yield return　new WaitForSeconds(10);

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

      
}
