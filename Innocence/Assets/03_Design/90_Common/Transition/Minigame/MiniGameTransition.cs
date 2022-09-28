using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MiniGameTransition : MonoBehaviour
{
    //遷移後のイベント
    [SerializeField]
    private UnityEvent OnComplete = null;

    //Animator
    Animator anim;

    //終了フラグ
    private bool isComplete = false;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    //遷移スタート
    public IEnumerator StartTransition()
    {
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Transition);
        //アニメーション
        anim.SetBool("isTransition", true);
        //isCompleteがtrueになるまで待機
        while (!isComplete)
        {
            yield return null;
        }
        //イベント
        if (OnComplete != null) { OnComplete.Invoke(); }
        //終了フラグを戻す
        isComplete = false;
    }

    //アニメーション終了
    public void CompleteAnim()
    {
        anim.SetBool("isTransition", false);
        isComplete = true;
    }
}
