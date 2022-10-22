using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scroll : MonoBehaviour
{
    [SerializeField] GameObject obj; //移動させる対象

    Vector3 targetPos; //移動先
    float currentY = 0f;　//移動先(y)

    [SerializeField]
    float upperScrollLimit = 0f;
    [SerializeField]
    float lowerScrollLimit = 0f;
    [SerializeField] float speed = 1.0f; //移動速度
    private void Start()
    {
        //初期化
        targetPos = obj.transform.position; //初期位置
        currentY = targetPos.y;
    }
    void Update()
    {
       
        //Debug.Log(targetPos != obj.transform.position);
        //Debug.Log(currentY <= 18f);
        //Debug.Log(currentY >= 250f);

       

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //上限
            if (currentY <= upperScrollLimit)
            {
                return;
            }
            //移動先設定
            currentY -= 18f; //Ý軸のみ変更
            targetPos.y = currentY;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //上限
            if (currentY >= lowerScrollLimit)
            {
                return;
            }
            //移動先設定
            currentY += 18f;
            targetPos.y = currentY;
        }

        //移動先!＝現在の位置の場合の時移動
        if (targetPos != obj.transform.position)
        {
            //移動
            //移動対象 = (移動元,移動先,速度)
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPos, speed * Time.deltaTime);
            return;
        }

    }
}
