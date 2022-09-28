using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StageInfo : MonoBehaviour
{
    [SerializeField] private Text stages = null;
    [SerializeField] private Text difficulty = null;
    [SerializeField] private Text stamina = null;
    [SerializeField] private Image stageImage = null;

    TextAsset stageCSV; //ステージの情報
    List<string[]> stageInfo = new List<string[]>();//ステージ情報のリスト

    private void Awake()
    {
        stageCSV = Resources.Load("CSV/StageInfo") as TextAsset;
        StringReader reader = new StringReader(stageCSV.text);

        // , で分割しつつ一行ずつ読み込み
        //リストに追加
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
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
