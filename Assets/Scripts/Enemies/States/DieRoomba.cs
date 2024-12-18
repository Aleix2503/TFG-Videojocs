using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRoomba : Die
{
    private bool alreadyDead = false;

    private new void Start()
    {
        base.Start();
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
        gameObject.SetActive(false);
    }

    public void setAlredyDead()
    {
        alreadyDead = false;
    }
}
