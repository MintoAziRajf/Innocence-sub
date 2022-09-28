using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAnimation : MonoBehaviour
{
    private float time = 0f;
    private float posY = 0f;

    private void FixedUpdate()
    {
        //鍵を上下に動かす
        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0f;
        }
        else if (time > 0.5f)
        {
            posY = -0.005f;
        }
        else
        {
            posY = +0.005f;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + posY, this.transform.position.z);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーが触れた瞬間鍵を消す
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.B_Key);
            this.gameObject.SetActive(false);
        }
    }
}
