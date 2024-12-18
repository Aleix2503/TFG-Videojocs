using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRoomba : Die
{
    private bool alreadyDead = false;
    private float previousSpeed;

    private new void Start()
    {
        base.Start();
        previousSpeed = GetComponent<Patrol>().patrolSpeed;
    }

    public override void Behaviour()
    {
        if (!alreadyDead) StartCoroutine(killRoomba());
    }

    private IEnumerator killRoomba()
    {
        animator.SetTrigger("isDead");
        alreadyDead = true;
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        alreadyDead = false;
        Instantiate(dieParticles, transform.position, dieParticles.transform.rotation);
        transform.parent.gameObject.SetActive(false);
    }
}
