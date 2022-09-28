using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        if (collider.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
        if (collider.CompareTag("EnemyBulletA"))
        {
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }
}
