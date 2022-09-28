using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
        if (collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
