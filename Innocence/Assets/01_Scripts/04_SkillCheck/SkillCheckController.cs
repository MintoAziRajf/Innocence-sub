using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckController : MonoBehaviour
{
    NodeController nodeController;
    SkillCheckManager skillCheckManager;

    //フラグ
    private bool isStart = false;
    public bool IsStart
    {
        get { return this.isStart; }
        set { this.isStart = value; }
    }

    //操作できる回数
    private int energy;
    public int Energy
    {
        set { energy = value; }
    }
    //タイミングよくノーツを叩けた回数
    private int successNode = 0;
    public int SuccessNode
    {
        get { return successNode; }
    }

    private bool isCollision = false; //ノーツに衝突してるかどうか
    private GameObject currentNode = null; //衝突しているノーツ

    // 移動関連
    private GameObject playerVisual = null; //見た目を回転させる用
    private GameObject mCamera = null; //カメラ移動用
    [SerializeField] private Transform cameraLookAt = null; //カメラの視先
    private float speed = -10f; //回転速度(何秒かけるか)

    //ステージの中心
    private Vector3 center = Vector3.zero;
    //回転軸
    private Vector3 axis = Vector3.forward;

    void OnEnable()
    {
        //ミニゲームを管理するスクリプトを取得
        skillCheckManager = GameObject.Find("SkillCheckManager").GetComponent<SkillCheckManager>();
        //見た目オブジェクトを取得
        playerVisual = transform.Find("Visual").gameObject;
        //カメラオブジェクトを取得
        mCamera = Camera.main.gameObject;
        //ステージの中心を取得
        center = skillCheckManager.Center;
        LookAtThis(cameraLookAt.position, mCamera.transform);
    }

    void Update()
    {
        if (!isStart) return;
        LookAtThis(center, this.transform);
        LookAtThis(cameraLookAt.position, mCamera.transform);
        RotatePosition(this.transform);
        RotatePosition(mCamera.transform);
        RotatePosition(cameraLookAt);
        RotatePlayer();
        PlayerControll();
    }

    private void PlayerControll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (energy == 0)
            {
                //空撃ちのような音
                Debug.Log("SkillCheck:空撃ち");
                return;
            }
            //回数消費
            Tapped();
            if (isCollision)
            {
                Debug.Log("Good");
                //成功処理
                skillCheckManager.Quota = skillCheckManager.Quota - 1;
                nodeController.NodeSuccess();
                //SE
                SoundManager.instance.PlaySE(SoundManager.SE_Type.SC_Success);
            }
            else
            {
                Debug.Log("Miss");
                //失敗処理
                //SE
                
            }
        }
    }

    private void Tapped()
    {
        //回数消費
        energy--;
        //UI表示
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Node"))
        {
            Debug.Log("nodeOn");
            currentNode = other.gameObject;
            nodeController = currentNode.GetComponent<NodeController>();
            isCollision = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Node"))
        {
            Debug.Log("nodeOff");
            currentNode = null;
            nodeController.NodeExit();
            nodeController = null;
            isCollision = false;
        }
    }

    /// <summary>
    /// 円運動
    /// </summary>
    /// <param name="target">対象のオブジェクト</param>
    private void RotatePosition(Transform target)
    {
        var angleAxis = Quaternion.AngleAxis(360 / speed * Time.deltaTime, axis); //角度
        var pos = target.position; //現在の座標
        //移動先を計算
        pos -= center; 
        pos = angleAxis * pos;
        pos += center;
        //移動
        target.position = pos;
    }

    /// <summary>
    /// プレイヤーの見た目を回転させる
    /// </summary>
    private void RotatePlayer()
    {
        var rot = playerVisual.transform.localEulerAngles; //現在の角度
        rot.x = Mathf.Repeat(rot.x + 1f, 90); //角度計算
        playerVisual.transform.localEulerAngles = rot; //回転
    }

    /// <summary>
    /// 中心を向く
    /// </summary>
    private void LookAtThis(Vector3 targetPos, Transform player)
    {
        // 中心へ方向
        Vector3 relativePos = targetPos - player.position;
        // 方向を、回転情報に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        player.rotation = rotation;
    }
}
