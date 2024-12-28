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
            //gameObject.GetComponent<FSMEnemies>().state = FSMEnemies.State.Hit;

            float directionX = transform.localPosition.x > 0.2 ? Mathf.Sign(transform.position.x - GetComponentInParent<Transform>().position.x) : 0f;
            float directionY = transform.localPosition.y > 0.2 || transform.localPosition.y < -0.2 ? Mathf.Sign(transform.position.y - GetComponentInParent<Transform>().position.y) : 0f;

            Vector2 hitDirection = new Vector2(directionX, directionY); 
            gameObject.GetComponent<Hit>().hitDirectionVector(hitDirection);
        }
    }
}
