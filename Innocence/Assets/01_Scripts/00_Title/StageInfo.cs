using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class StageInfo : MonoBehaviour
{
    [SerializeField] private Text stages = null;
    [SerializeField] private Text difficulty = null;
    [SerializeField] private Text stamina = null;
    [SerializeField] private Image stageImage = null;

    List<string[]> stageInfo = new List<string[]>();//ステージ情報のリスト

    private void Awake()
    {
        string path = Application.dataPath + @"/08_CSV/StageInfo.csv"; //ステージ情報の格納場所
        StreamReader csv = new StreamReader(path, Encoding.UTF8);　//StreamReaderで読み込み
        string line = null;
        // , で分割しつつ一行ずつ読み込み
        //リストに追加
        while ((line = csv.ReadLine()) != null)
        {
            stageInfo.Add(line.Split(','));
        }
    }

    public void SelectingStage(int num)
    {
        stages.text = stageInfo[num][0];//ステージ名
        difficulty.text = stageInfo[num][1];//難易度
        //難易度によって文字の色を変える
        switch(difficulty.text)
        {
            case "Easy":
                difficulty.color = Color.green;
                break;
            case "Normal":
                difficulty.color = Color.yellow;
                break;
            case "Hard":
                difficulty.color = Color.red;
                break;
            case "Nightmare":
                difficulty.color = Color.blue;
                break;
            default:
                break;
        }
        stamina.text = stageInfo[num][2];//歩数
        stageImage.sprite = Resources.Load<Sprite>("Sprite/00_Title/Stage_" + num.ToString("00"));//ステージのサンプル画像
    }
}
