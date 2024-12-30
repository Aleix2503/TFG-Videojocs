using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRoomba : Hit
{
    private bool alreadyHit = false;

    private float previousSpeed;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        previousSpeed = GetComponent<PatrolRoomba>().patrolSpeed;
    }

    protected override void Behaviour()
    {
        if (!alreadyHit) StartCoroutine(hit());
    }

    private IEnumerator hit()
    {
        alreadyHit = true;
        
        GetComponent<Patrol>().patrolSpeed = 0;

        animator.SetTrigger("isHit");

        yield return new WaitForSeconds(hitTime);

        alreadyHit = false;

        GetComponent<Patrol>().patrolSpeed = previousSpeed;

        animator.SetTrigger("isPatroled");

        fsmEnemies.state = FSMEnemies.State.Patrol;
    }

    public void setAlreadyHit()
    {
        alreadyHit = false;
    }
}
