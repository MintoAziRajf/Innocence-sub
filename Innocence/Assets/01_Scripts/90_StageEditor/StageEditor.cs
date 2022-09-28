using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class StageEditor : MonoBehaviour
{
    //Dropdown変数
    [SerializeField] private Dropdown stageDropdown = null; //ステージ数
    [SerializeField] private Dropdown[] mapDropdown = null; //マップの配置
    [SerializeField] private Dropdown[] typeDropdown = null; //ミニゲームの種類

    //enum
    private enum MAP //マップに配置するオブジェクト
    {
        NONE,
        STONE,
        WALL,
        BUTTON,
        TRAP,
        PLAYER,
        ERROR
    }

    //Textから情報を持ってくる用
    [SerializeField] private Text stepsText = null; //歩数
    [SerializeField] private Text[] diffText = null; //ミニゲームの難易度

    public void Confirm()
    {
        StreamWriter sw;
        FileInfo fi;
        string s = null;
        string path = null;
        
        // 指定したCSVへステージ情報を保存
        path = Application.dataPath + "/Resources/CSV/map_" + stageDropdown.value.ToString("0") + ".csv";
        fi = new FileInfo(path);
        sw = fi.CreateText();
        for (int i = 0; i < 6; i++) //i = x, j= y
        {
            for (int j = 0; j < 7; j++)
            {
                switch(mapDropdown[7*i + j].value)
                {
                    case (int)MAP.NONE:
                        s = "O";
                        break;
                    case (int)MAP.STONE:
                        s = "S";
                        break;
                    case (int)MAP.WALL:
                        s = "W";
                        break;
                    case (int)MAP.BUTTON:
                        s = "B";
                        break;
                    case (int)MAP.TRAP:
                        s = "T";
                        break;
                    case (int)MAP.PLAYER:
                        s = "P";
                        break;
                    default:
                        Debug.Log("不適切な値です。" + mapDropdown[7*i + j].value);
                        break;
                }
                if (j == 6) break;　//最後の行のみコンマで区切らない
                sw.Write(s + ",");
            }
            sw.WriteLine();
        }
        sw.WriteLine("Steps");
        s = stepsText.text;
        sw.WriteLine(s);
        sw.Flush();

        // 指定したCSVへステージ情報を保存
        path = Application.dataPath + "/Resources/CSV/gameTest_" + stageDropdown.value.ToString("0") + ".csv";
        fi = new FileInfo(path);
        sw = fi.CreateText();
        Debug.Log("a");
        sw.WriteLine("Type,Difficulty");
        for(int i = 0; i < 9; i++)
        {
            s = typeDropdown[i].value.ToString("0") + ",";
            sw.Write(s);
            s = diffText[i].text;
            sw.WriteLine(s);
        }
        sw.Flush();
        sw.Close();
    }

    void Awake()
    {
    }
}

