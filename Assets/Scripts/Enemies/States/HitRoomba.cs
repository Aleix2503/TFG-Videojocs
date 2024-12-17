using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRoomba : Hit
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
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
