using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public static SoundManager instance;

    // BGM管理
    public enum BGM_Type
    {
        Title,
        MainGame,
        Shooting,
        Timing,
        Battle,
        Ending,
        Opening
    }

    // SE管理
    public enum SE_Type
    {
        // SE登録
        GameOver,
        Kick,
        Move,
        Select,
        Submit,

        Shutter,
        Ironbars,
        Bullet,
        Transition,
        S_Damage,

        S_Destroy,
        B_Chain_1,
        B_Chain_2,
        B_Thunder,
        T_Start,

        T_Move,
        T_Goal,
        M_Goal,
        B_Key,
        M_Switch,

        B_Damage
    }

    // クロスフェード時間
    public const float CROSS_FADE_TIME = 0f;

    // ボリューム関連
    private float BGM_Volume = 0.1f;
    private float SE_Volume = 0.2f;

    // AudioClip
    public AudioClip[] BGM_Clips;
    public AudioClip[] SE_Clips;

    // AudioSource
    private AudioSource[] BGM_Sources = new AudioSource[2];
    private AudioSource[] SE_Sources = new AudioSource[16];

    private bool isCrossFading;

    private int currentBgmIndex = 999;

    void Awake()
    {
        // シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // BGM用 AudioSource追加
        BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
        BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

        // SE用 AudioSource追加
        for (int i = 0; i < SE_Sources.Length; i++)
        {
            SE_Sources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ボリューム設定
        if (!isCrossFading)
        {
            BGM_Sources[0].volume = BGM_Volume;
            BGM_Sources[1].volume = BGM_Volume;
        }

        foreach (AudioSource source in SE_Sources)
        {
            source.volume = SE_Volume;
        }
    }

    // BGM再生
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
    {
        // BGMなしの状態にする場合            
        if ((int)bgmType == 999)
        {
            StopBGM();
            return;
        }

        int index = (int)bgmType;
        currentBgmIndex = index;

        if (index < 0 || BGM_Clips.Length <= index)
        {
            return;
        }

        // 同じBGMの場合は何もしない
        if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGM_Clips[index])
        {
            return;
        }
        else if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGM_Clips[index])
        {
            return;
        }

        // フェードでBGM開始
        if (BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
        {
            BGM_Sources[0].loop = loopFlg;
            BGM_Sources[0].clip = BGM_Clips[index];
            BGM_Sources[0].Play();
        }
        else
        {
            // クロスフェード処理
            StartCoroutine(CrossFadeChangeBGM(index, loopFlg));
        }
    }

    // BGMのクロスフェード処理
    private IEnumerator CrossFadeChangeBGM(int index, bool loopFlg)
    {
        isCrossFading = true;
        if (BGM_Sources[0].clip != null)
        {
            // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            BGM_Sources[1].volume = 0;
            BGM_Sources[1].clip = BGM_Clips[index];
            BGM_Sources[1].loop = loopFlg;
            BGM_Sources[1].Play();
            BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[0].Stop();
            BGM_Sources[0].clip = null;
        }
        else
        {
            // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
            BGM_Sources[0].volume = 0;
            BGM_Sources[0].clip = BGM_Clips[index];
            BGM_Sources[0].loop = loopFlg;
            BGM_Sources[0].Play();
            BGM_Sources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[1].Stop();
            BGM_Sources[1].clip = null;
        }
        isCrossFading = false;
    }

    // BGM完全停止
    public void StopBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
        BGM_Sources[0].clip = null;
        BGM_Sources[1].clip = null;
    }

    // SE再生
    public void PlaySE(SE_Type seType)
    {
        int index = (int)seType;
        if (index < 0 || SE_Clips.Length <= index)
        {
            return;
        }

        // 再生中ではないAudioSouceをつかってSEを鳴らす
        foreach (AudioSource source in SE_Sources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE_Clips[index];
                source.Play();
                return;
            }
        }
    }

    // SE停止
    public void StopSE()
    {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in SE_Sources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    // BGM一時停止
    public void MuteBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
    }

    // 一時停止した同じBGMを再生(再開)
    public void ResumeBGM()
    {
        BGM_Sources[0].Play();
        BGM_Sources[1].Play();
    }

    // BGMVolume設定
    public void SetVolumeBGM(float vol)
    {
        BGM_Volume = vol;
    }

    // SEVolume設定v
    public void SetVolumeSE(float vol)
    {
        SE_Volume = vol;
    }


    /*使い方
     * SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Title);　　　<=　引数を変える
     * SoundManager.instance.PlaySE(SoundManager.SE_Type.OK);　　　　　 <=　引数を変える
     * SoundManager.instance.StopBGM();
     * SoundManager.instance.StopSE();
     * SoundManager.instance.MuteBGM();
     * SoundManager.instance.ResumeBGM();
     */
}
