using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scroll : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform; //移動させる対象

    Vector2 targetPos; //移動先
    float currentY = 0f;　//移動先(y)

    [SerializeField] float speed = 1.0f; //移動速度
    [SerializeField] float topLimit = 0f, downLimit = 0f; //上限、下限
    [SerializeField] float moveRange = 0f; //移動距離
    private void Start()
    {
        //初期化
        targetPos = rectTransform.anchoredPosition; //初期位置
        currentY = targetPos.y;
    }
    void Update()
    {
        //移動
        //移動対象 = (移動元,移動先,速度)
        rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetPos, speed * Time.deltaTime);

        //移動先!＝現在の位置の場合の時移動
        if (targetPos == rectTransform.anchoredPosition)
        {
            Debug.Log("StageSelect:操作可能です。");
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("a");
                //上限
                if (currentY <= topLimit)
                {
                    Debug.Log("StageSelect:上限です。");
                    return;
                }
                //移動先設定
                currentY -= moveRange; //Ý軸のみ変更
                targetPos.y = currentY;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //上限
                if (currentY >= downLimit)
                {
                    Debug.Log("StageSelect:下限です。");
                    return;
                }
                //移動先設定
                currentY += moveRange;
                targetPos.y = currentY;
            }
        }
        
    }
}
