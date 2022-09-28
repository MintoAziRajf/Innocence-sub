using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollisionDetection : MonoBehaviour
{
    private enum DIRECTION 
    {
        TOP,
        RIGHT,
        UNDER,
        LEFT
    }
    //プレイヤーのどの方向に位置するか
    [SerializeField] DIRECTION direction = DIRECTION.TOP;

    //PlayerController
    PlayerController playerController;

    private void Awake()
    {
        playerController = this.gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Playerに衝突中の情報を送る
        if (collider.CompareTag("Stone") || collider.CompareTag("Switch"))
        {
            playerController.CollisionInfomation((int)direction, true, collider.gameObject);
        }
        if (collider.CompareTag("Wall"))
        {
            playerController.CollisionInfomation((int)direction, true, null);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //PlayerにExit判定を送る
        if (collider.CompareTag("Stone") || collider.CompareTag("Switch") || collider.CompareTag("Wall"))
        {
            playerController.CollisionInfomation((int)direction, false, null);
        }
    }
}
