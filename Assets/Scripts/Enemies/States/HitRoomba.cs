using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRoomba : Hit
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void Behaviour()
    {
        if (!alreadyHit) StartCoroutine(hit());
    }

    private IEnumerator hit()
    {
        alreadyHit = true;
        rb.velocity = Vector3.zero;
        animator.SetTrigger("isHit");

        yield return new WaitForSeconds(hitTime);

        fsmEnemies.state = FSMEnemies.State.Patrol;
    }
}
