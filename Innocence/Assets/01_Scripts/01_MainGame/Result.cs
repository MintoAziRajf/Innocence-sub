using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private enum RANK
    {
        GOOD = 0,
        GREAT = 5,
        EXCELLENT = 10
    }

    void OnEnable()
    {
        
    }

    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="remainingSteps">残り歩数</param>
    public void ResultData(int remainingSteps)
    {
        switch (CheckRank(remainingSteps))
        {
            case RANK.EXCELLENT: //星3
                //処理
                break;
            case RANK.GREAT: //星2
                //処理
                break;
            case RANK.GOOD: //星1
                //処理
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ランク判定
    /// </summary>
    /// <param name="num">残り歩数</param>
    /// <returns>
    /// 0-4:GOOD 
    /// 5-9:GREAT
    /// 10-
    /// </returns>
    private RANK CheckRank(int num)
    {
        if (num >= (int)RANK.EXCELLENT)
        {
            return RANK.EXCELLENT;
        }
        else if (num >= (int)RANK.GREAT)
        {
            return RANK.GREAT;
        }
        else
        {
            return RANK.GOOD;
        }
    }
}
