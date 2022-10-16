using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;//DebugLog用

public class MapCreater : MonoBehaviour
{
    List<string[]> gameDatas = new List<string[]>(); //gameのリスト
    List<string[]> mapDatas = new List<string[]>();  //mapのリスト

    [SerializeField] private GameObject blockPrefab = null; //お邪魔ブロックのPrefab
    [SerializeField] private GameObject stonePrefab = null; //ストーンのPrefab
    [SerializeField] private GameObject buttonPrefab = null; //ボタンのPrefab
    [SerializeField] private GameObject trapPrefab = null; //トラップのPrefab

    [SerializeField] private GameObject mapParent = null; //ブロックのInstantiate先
    CSVManager csvManager; //ステージ数を引っ張ってくる用

    StringBuilder sb = new StringBuilder(""); //Debug用
    private GameObject player; //Playerの情報書き換え用

    private enum TYPE
    {
        NONE ,
        STONE,
        WALL,
        BUTTON,
        TRAP,
        PLAYER,
        ERROR
    }

    private void Awake()
    {
        //Init
        player = GameObject.Find("Player");
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();

        LoadCSV(); //マップのデータをCSVから読み込む
        CreateMap(); //マップの生成
        player.GetComponent<PlayerController>().Steps = int.Parse(mapDatas[7][0]); //マップデータから歩数を持ってくる
    }

    /// <summary>
    ///マップの生成 
    /// </summary>
    private void CreateMap()
    {
        int game = 0; //Game判定用
        //7ｘ6のマップにブロックを配置する
        for (int i = 0; i < 6; i++)
        {
            GameObject obj;//生成後の位置調整,スクリプトを読み込む用
            sb.AppendLine(""); //Debug

            for (int j = 0; j < 7; j++)
            {
                TYPE type = ReturnTYPE(mapDatas[i][j]); //blockID

                //NONE:何も生成しない
                //BLOCK:お邪魔ブロック生成
                //SWITCH:スイッチを生成
                //TRAP:罠の生成
                //PLAYER:プレイヤーの位置を変更する
                //その他:stoneを生成,StoneControllerに変数の受け渡し
                switch (type)
                {
                    case TYPE.NONE:
                        sb.Append("〇");//Debug
                        break;
                    case TYPE.WALL:
                        sb.Append("壁");//Debug
                        obj = Instantiate(blockPrefab, mapParent.transform, false); //オブジェクト生成
                        obj.transform.localPosition = new Vector3(j, -i, 0f);  //位置書き換え
                        break;
                    case TYPE.BUTTON:
                        sb.Append("押");//Debug
                        obj = Instantiate(buttonPrefab, mapParent.transform, false); //オブジェクト生成
                        obj.transform.localPosition = new Vector3(j, -i, 0f);  //位置書き換え
                        break;
                    case TYPE.TRAP:
                        sb.Append("罠");//Debug
                        obj = Instantiate(trapPrefab, mapParent.transform, false); //オブジェクト生成
                        obj.transform.localPosition = new Vector3(j, -i, 0f);  //位置書き換え
                        break;
                    case TYPE.STONE:
                        sb.Append("石");//Debug
                        int t = int.Parse(gameDatas[game][0]); //ゲームのタイプ
                        int d = int.Parse(gameDatas[game][1]); //ゲームの難易度
                        obj = Instantiate(stonePrefab, mapParent.transform, false); //オブジェクト生成
                        obj.GetComponent<StoneController>().SetStoneInfo(t, d); //タイプと難易度の受け渡し
                        obj.transform.localPosition = new Vector3(j, -i, 0f);  //位置書き換え
                        game++;
                        break;
                    case TYPE.PLAYER:
                        sb.Append("人");//Debug
                        player.transform.localPosition = new Vector3(j, -i, 0f);  //位置書き換え
                        break;
                }
            }
        }
        Debug.Log(sb);
    }

    private TYPE ReturnTYPE(string str)
    {
        TYPE t;
        switch (str)
        {
            case "O":
                t = TYPE.NONE;
                break;
            case "W":
                t = TYPE.WALL;
                break;
            case "S":
                t = TYPE.STONE;
                break;
            case "B":
                t = TYPE.BUTTON;
                break;
            case "T":
                t = TYPE.TRAP;
                break;
            case "P":
                t = TYPE.PLAYER;
                break;
            default:
                Debug.Log("不正な値です。:" + str);
                t = TYPE.ERROR;
                break;
        }
        return t;
    }

    /// <summary>
    /// CSVからマップの情報をロードする
    /// </summary>
    private void LoadCSV()
    {
        mapDatas = csvManager.MainDatas;
        gameDatas = csvManager.SubDatas;
    }
}
