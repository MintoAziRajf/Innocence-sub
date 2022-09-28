using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingController : MonoBehaviour
{
    //Scene遷移
    [SerializeField] private GameObject loadPrefab = null;

    //Animations
    [SerializeField] private GameObject jailerObj = null;
    private float x, y;
    private Vector3 pos;

    //
    [SerializeField] private GameObject message = null;

    void Start()
    {
        //BGM
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Ending);
        pos = jailerObj.transform.position;
        StartCoroutine(Title());
    }

    private void Update()
    {
        x = 0.05f * Mathf.Cos(Time.time * 2.0f);      //X軸の設定
        y = 0.05f * Mathf.Sin(Time.time * 2.0f);      //Y軸の設定
        jailerObj.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
    }

    private IEnumerator Title()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        message.SetActive(true);
        //Spaceを押したらタイトルに戻る
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
        //SceneLoad
        GameObject SceneLoader = Instantiate(loadPrefab);
        Loading loading = SceneLoader.GetComponent<Loading>();
        loading.StartCoroutine("SceneLoading", "00_Title");
    }
}
