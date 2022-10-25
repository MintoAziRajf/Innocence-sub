using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] private GameObject loadPrefab = null;
    private void Start()
    {
        //BGM
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Opening);
        //スペースキーを押したらタイトルに進む
        StartCoroutine(WaitSpaceKey());
    }

    private IEnumerator WaitSpaceKey()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Submit);
        GameObject SceneLoader = Instantiate(loadPrefab);
        Loading loading = SceneLoader.GetComponent<Loading>();
        loading.StartCoroutine("SceneLoading", "00_Title");
    }
}
