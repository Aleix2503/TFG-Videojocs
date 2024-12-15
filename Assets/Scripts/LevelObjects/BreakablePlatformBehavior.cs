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
            if (playerController.stateMachine.currentBehaviour is CubedPlayerBehaviour) {
                CubedPlayerBehaviour cubedPlayerState = (CubedPlayerBehaviour)playerController.stateMachine.currentBehaviour;
                if (cubedPlayerState.canBreakGround)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
