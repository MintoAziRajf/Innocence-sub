using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCursolSound : MonoBehaviour
{
    //カーソル移動取得する用
    [SerializeField] private EventSystem eventSystem = null;
    private GameObject nowSelected;
    private GameObject oldSelected;

    void OnEnable()
    {
        SelectInitialization();
    }

    void Update()
    {
        if(nowSelected == null)
        {
            SelectInitialization();
        }

        JudgeMoveCursol();
    }

    //カーソル移動判定
    private void JudgeMoveCursol()
    {
        nowSelected = eventSystem.currentSelectedGameObject;
        if (oldSelected != nowSelected)
        {
            //カーソル移動音
            SoundManager.instance.PlaySE(SoundManager.SE_Type.Select);
        }
        oldSelected = nowSelected;
    }

    //選択中のオブジェクトの初期化
    private void SelectInitialization()
    {
        nowSelected = eventSystem.currentSelectedGameObject;
        oldSelected = eventSystem.currentSelectedGameObject;
    }

}
