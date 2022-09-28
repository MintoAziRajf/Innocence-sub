using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTimingMap : MonoBehaviour
{
    [SerializeField, Range(0, 2),Tooltip("0:回転 1:横移動 2:縦移動")] private int moveType = 0;
    [SerializeField,Tooltip("回転速度もしくは移動距離")] private float value = 0f;
    private Vector3 prevPos;
    private Vector3 move;
    private float time = 0f;

    void Start()
    {
        switch (moveType)
        {
            case 0:
                move = new Vector3(0f, value, 0f);
                break;
            case 1:
                move = new Vector3(value, 0f, 0f);
                prevPos = this.transform.position;
                move = move + prevPos;
                break;
            case 2:
                move = new Vector3(0f, 0f, value);
                prevPos = this.transform.position;
                move = move + prevPos;
                break;
        }
        
    }
    void FixedUpdate()
    {
        switch (moveType)
        {
            case 0:
                this.transform.Rotate(move);
                break;
            case 1:
                time += Time.deltaTime;
                if (time > 2f)
                {
                    Vector3 tmpVec = prevPos;
                    prevPos = move;
                    move = tmpVec;
                    time = 0f;
                }
                this.transform.position = Vector3.Lerp(prevPos, move, time);
                break;
            case 2:
                time += Time.deltaTime;
                if (time > 2f)
                {
                    Vector3 tmpVec = prevPos;
                    prevPos = move;
                    move = tmpVec;
                    time = 0f;
                }
                this.transform.position = Vector3.Lerp(prevPos, move, time);
                break;
        }
    }
}
