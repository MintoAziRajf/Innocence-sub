﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Image[] missionImage = new Image[3];
    [SerializeField] private Sprite successSprite = null;
    [SerializeField] private Image rankImage = null;
    [SerializeField] private Image rankImage_sub = null;
    [SerializeField] private Sprite[] rankSprite = new Sprite[3];

    [SerializeField] private Text stageTitle = null;
    [SerializeField] private Text stagesText = null;

    Animator anim;

    private enum RANK
    {
        GOOD = 0,
        GREAT = 1,
        EXCELLENT = 2
    }

    void OnEnable()
    {
        anim = this.GetComponent<Animator>();
    }

    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="remainingSteps">残り歩数</param>
    public void ResultData(int remainingSteps,int stages, string name)
    {
        stageTitle.text = name;
        stagesText.text = "STAGE-" + stages.ToString("00");

        RANK rank = CheckRank(remainingSteps); 　　　 //ランク判定
        anim.SetInteger("Rank",(int)rank); 　　　　   //ランクに応じたアニメーション起動
        rankImage.sprite = rankSprite[(int)rank];     //ランクに応じてrankSprite[]を表示
        rankImage_sub.sprite = rankSprite[(int)rank]; //ランクに応じてrankSprite[]を表示

        missionImage[0].sprite = successSprite; //星１
        if (rank == RANK.GOOD) return;
        missionImage[1].sprite = successSprite; //星２
        if (rank == RANK.GREAT) return;
        missionImage[2].sprite = successSprite; //星３
    }

    /// <summary>
    /// ランク判定
    /// </summary>
    /// <param name="num">残り歩数</param>
    /// <returns>
    /// 0-4:GOOD 
    /// 5-9:GREAT
    /// 10-:EXCELLENT
    /// </returns>
    private RANK CheckRank(int num)
    {
        if (num >= 10) 
        {
            return RANK.EXCELLENT;
        }
        else if (num >= 5)
        {
            return RANK.GREAT;
        }
        else
        {
            return RANK.GOOD;
        }
    }
}
