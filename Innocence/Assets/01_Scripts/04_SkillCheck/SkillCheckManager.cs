using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckManager : MiniGameMonoBehaviour
{
    //Debug
    public int debugDiff = 0;
    public bool isDebug = false;
    //GameManager
    MainGameManager mGameManager;
    SkillCheckController scc;

    [SerializeField] private GameObject nodePrefab = null;

    private float time = 5f;  //制限時間

    //フラグ
    private bool isStart = false;
    public bool IsStart
    {
        get { return this.isStart; }
        set { this.isStart = value; }
    }

    //ステージの中心
    [SerializeField] private Vector3 center = Vector3.zero;
    public Vector3 Center
    {
        get { return center; }
    }
    //回転軸
    private Vector3 axis = Vector3.forward;

    //難易度情報
    CSVManager csvManager;
    private string[] gameInfo;

    //エネルギー(プレイヤーが操作できる回数)
    private int energy = 0;
    public int Energy
    {
        get { return energy; }
        set { energy = value; DisplayInterface(); }
    }
    //クリアノルマ回数
    private int quota = 0;
    public int Quota
    {
        get { return quota; }
        set { quota = value; if (value <= 0) quota = 0; DisplayInterface(); }
    }

    //UI
    [SerializeField] private GameObject clearUI = null;
    [SerializeField] private TextMesh quotaText = null; //ノルマ表示用
    [SerializeField] private TextMesh energyText = null;//エネルギー表示用

    void OnEnable()
    {
        csvManager = GameObject.Find("CSVManager").GetComponent<CSVManager>();
        scc = GameObject.Find("Player").GetComponent<SkillCheckController>();
        if (!isDebug)
        {
            mGameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();
            gameInfo = csvManager.SkillCheckInfo(mGameManager.Difficulty);
        }
        else
        {
            gameInfo = csvManager.SkillCheckInfo(debugDiff);
        }
        SetDifficulty();
        CreateNode();
        StartCoroutine(StartDelay());
        DisplayInterface();
    }

    void Update()
    {
        if (!isStart) return;
        if (time <= 0f)
        {
            //GameEnd
            EndGame();
        }
        time -= Time.deltaTime;
    }

    private void EndGame()
    {
        isStart = false;
        scc.IsStart = false;
        if (quota == 0)
        {
            Debug.Log("成功");
            //ClearUIをアクティブ
            clearUI.SetActive(true);
            //シーンのアンロード,ゲームマネージャーに成否を送る
            mGameManager.StartCoroutine("UnloadScene", true);
        }
        else
        {
            Debug.Log("失敗");
            //シーンのアンロード,ゲームマネージャーに成否を送る
            mGameManager.StartCoroutine("UnloadScene", false);
        }
    }

    /// <summary>
    /// ノーツの生成
    /// </summary>
    private void CreateNode()
    {
        float angle = 0f;
        for (int i = 0; i < energy; i++)
        {
            angle = float.Parse(gameInfo[3 + i]);
            GameObject node = Instantiate(nodePrefab, this.transform);
            var angleAxis = Quaternion.AngleAxis(angle, axis);
            var pos = node.transform.position;
            pos -= center;
            pos = angleAxis * pos;
            pos += center;
            node.transform.position = pos;
        }
    }

    /// <summary>
    /// 難易度からゲームを設定する
    /// </summary>
    private void SetDifficulty()
    {
        //タップ可能回数を設定
        energy = int.Parse(gameInfo[1]);
        quota = int.Parse(gameInfo[2]);
        scc.Energy = energy;
    }

    //読み込んでからミッション表示後ゲームスタート
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(3.0f);
        //SE
        //SoundManager.instance.PlaySE(SoundManager.SE_Type.T_Start);
        //ゲームスタート
        isStart = true;
        scc.IsStart = true;
    }

    //UI表示
    private void DisplayInterface()
    {
        quotaText.text = quota.ToString("00");
        energyText.text = energy.ToString("00");
    }
}
