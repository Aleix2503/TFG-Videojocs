using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject gameObject = collision.gameObject;
            gameObject.GetComponent<FSMEnemies>().state = FSMEnemies.State.Hit;
        }
    }
}
