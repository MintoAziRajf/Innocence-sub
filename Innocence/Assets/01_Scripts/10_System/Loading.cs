using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    Animator animator;
    Canvas myCanvas;
    void Awake()
    {
        //rednderCameraの設定
        myCanvas = GetComponent<Canvas>();
        if (myCanvas.worldCamera == null)
        {
            myCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        animator = GetComponent<Animator>();
    }

    public IEnumerator SceneLoading(string sceneName)
    {
        Time.timeScale = 0f;
        StartCoroutine(PlaySE());
        animator.SetTrigger("LoadOut");
        // 非同期でロードを行う
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // ロードが完了していても，シーンのアクティブ化は許可しない
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) //0.9で止まる
        {
            yield return 0;
        }
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1f;
        // ロードが完了したときにシーンのアクティブ化を許可する
        asyncLoad.allowSceneActivation = true;

        // ロードが完了するまで待つ
        yield return asyncLoad;
    }

    private IEnumerator PlaySE()
    {
        //SE
        yield return new WaitForSecondsRealtime(0.5f);
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Ironbars);
    }
}
