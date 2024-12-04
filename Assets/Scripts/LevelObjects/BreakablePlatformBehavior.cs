using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatformBehavior : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            print(rb2D.velocity.y);
            /*if (playerController.stateMachine.currentBehaviour is PlayerExpandedState) {
                PlayerExpandedState expandedState = (PlayerExpandedState)playerController.stateMachine.currentBehaviour;
                if (expandedState.canBreakGround)
                {
                    Destroy(gameObject);
                }
            }*/
        }
    }
}
