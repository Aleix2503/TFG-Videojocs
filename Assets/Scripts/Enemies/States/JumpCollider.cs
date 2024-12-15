using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollider : MonoBehaviour
{
    private FSMEnemies fsmEnemies;
    private Animator animator;

    private void Start()
    {
        fsmEnemies = GetComponentInParent<FSMEnemies>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && fsmEnemies.state == FSMEnemies.State.Alert)
        {
            animator.SetTrigger("isAttacked");
            fsmEnemies.state = FSMEnemies.State.Attack;
        }
    }
}
