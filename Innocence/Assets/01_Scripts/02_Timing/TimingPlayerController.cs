using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingPlayerController : MonoBehaviour
{
    //移動スピード
    [SerializeField] private float speed = 1f;
    private Vector3 move;

    //ゲーム開始フラグ
    private bool isStart = false;

    //カウントダウン
    [SerializeField] private Text countDownText = null;

    //TimingManager 
    TimingManager timingManager;

    //GoalEffect
    [SerializeField] private GameObject goalObj = null;
    [SerializeField] private Material goalMaterial = null;
    //クリアアニメーション
    [SerializeField] private GameObject completeCanvas = null;

    //GameOverEffect
    [SerializeField] private GameObject gameoverEffect = null;

    private void Awake()
    {
        //移動方向の初期化
        move = new Vector3(speed, 0f, 0f);
    }

    private void Start()
    {
        timingManager = GameObject.Find("TimingManager").GetComponent<TimingManager>();
        StartCoroutine(StartDelay());
    }
    private void Update()
    {
        if (!isStart) return;

        if (Input.GetButtonDown("Up"))
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Move);
            move = new Vector3(0f, 0f, speed);
        }
        if (Input.GetButtonDown("Down"))
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Move);
            move = new Vector3(0f, 0f, -speed);
        }
        if (Input.GetButtonDown("Right"))
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Move);
            move = new Vector3(speed, 0f, 0f);
        }
        if (Input.GetButtonDown("Left"))
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Move);
            move = new Vector3(-speed, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        //スタートフラグがtrueになっている間指定方向に移動。
        if (!isStart) return;
        transform.position += move * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isStart) return;
        //失敗
        if (collider.CompareTag("Wall"))
        {
            StartCoroutine(DeadAnimation());
            isStart = false;
            timingManager.StartCoroutine("EndGame", false);
        }
        //成功
        if (collider.CompareTag("Goal"))
        {
            StartCoroutine(GoalAnimation());
            isStart = false;
            timingManager.StartCoroutine("EndGame", true);
        }
    }

    private IEnumerator DeadAnimation()
    {
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.S_Destroy);
        //Effect
        Instantiate(gameoverEffect, this.transform.position, this.transform.rotation, this.transform);
        //Player透明化
        this.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator GoalAnimation()
    {
        //ゴールの色を変える
        goalObj.GetComponent<Renderer>().material = goalMaterial;
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Goal);
        //プレイヤーをゴールの真ん中の位置に移動させる
        for (int i = 0; i < 30; i++)
        {
            yield return null;
            this.transform.position = Vector3.Lerp(this.transform.position, goalObj.transform.position, i / 30f);
        }
        //CompleteUIをアクティブ
        completeCanvas.SetActive(true);
    }


    //読み込んでから0.5秒以後にSpaceキーで開始
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "";
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Start);
        //ゲームスタート
        isStart = true;
    }
}
