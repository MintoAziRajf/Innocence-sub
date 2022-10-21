using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckCamera : MonoBehaviour
{
    //ステージの中心
    [SerializeField] private Transform cameraLookAt = null;
    [SerializeField] private Vector3 center = Vector3.zero;
    private float speed = -10f;
    //回転軸
    private Vector3 axis = Vector3.forward;

    private void Start()
    {
        LookCenter();
    }

    private void Update()
    {
        LookCenter();
        RotatePosition(this.transform);
        RotatePosition(cameraLookAt);
    }

    /// <summary>
    /// 中心を向く
    /// </summary>
    private void LookCenter()
    {
        // 中心へ方向
        Vector3 relativePos = cameraLookAt.position - this.transform.position;
        // 方向を、回転情報に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        this.transform.rotation = rotation;
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
}
