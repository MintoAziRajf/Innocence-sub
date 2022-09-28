using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameBGM : MonoBehaviour
{
    void OnEnable()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.MainGame);
    }
}
