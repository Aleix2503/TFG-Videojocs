using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieByEnemy : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerController.stateMachine.ChangeState(playerController.deathState);
        }
    }
}
