using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem = null;
    [SerializeField] GameObject bgmButton = null;
    [SerializeField] GameObject seButton = null;

    //選択中button格納用
    private GameObject selectedObj;

    private int seVolume = 0;
    private int bgmVolume = 0;

    private void Update()
    {
        selectedObj = eventSystem.currentSelectedGameObject.gameObject;
        if (selectedObj == bgmButton)
        {
            bgmVolume = JudgeSelect(bgmVolume);
            //SoundManager.instance.SetBgmVolume(bgmVolume);
            return;
        }
        if (selectedObj == seButton)
        {
            seVolume = JudgeSelect(seVolume);
            //SoundManager.instance.SetBgmVolume(seVolume);
            return;
        }

    }

    private int JudgeSelect(int vol)
    {
        if (Input.GetButtonDown("Right"))
        {
            return Mathf.Clamp(vol+1, 0, 3);
        }
        else if (Input.GetButtonDown("Left"))
        {
            return Mathf.Clamp(vol-1, 0, 3);
        }
        else
        {
            return vol;
        }
    }
}
