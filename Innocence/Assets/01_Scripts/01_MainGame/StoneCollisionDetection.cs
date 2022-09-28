using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCollisionDetection : MonoBehaviour
{
    //0:上 1:右 2:下 3:左
    [SerializeField] private int directionNum = 0;

    //PlayerController
    StoneController stoneController;
    private void Awake()
    {
        stoneController = this.gameObject.transform.parent.gameObject.GetComponent<StoneController>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {   
        if (collider.CompareTag("Wall") || collider.CompareTag("Stone") || collider.CompareTag("Trap") || collider.CompareTag("Switch") || collider.CompareTag("Goal"))
        {
            if(stoneController == null) return;
            stoneController.CollisionInfomation(directionNum, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall") || collider.CompareTag("Stone") || collider.CompareTag("Trap") || collider.CompareTag("Switch") || collider.CompareTag("Goal"))
        {
            if (stoneController == null) return;
            stoneController.CollisionInfomation(directionNum, false);
        }
    }
}
