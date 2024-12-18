using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover : StateBehaviour
{
    [SerializeField]
    private float timeRecover = 3;

    private bool alreadyCalled = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public override void Behaviour()
    {
        if (!alreadyCalled)
            StartCoroutine(jumpRecover());
    }

    private IEnumerator jumpRecover()
    {
        alreadyCalled = true;
        ResetAllTriggers(animator);

        yield return new WaitForSeconds(timeRecover);

        if (GetComponent<Patrol>() != null)
            fsmEnemies.state = FSMEnemies.State.Patrol;
        else if (GetComponent<Idle>() != null)
        {
            fsmEnemies.state = FSMEnemies.State.Idle;
            GetComponent<Idle>().SetAlreadyArrivedFalse();

            animator.SetTrigger("isIdled");
        } 
        else fsmEnemies.state = FSMEnemies.State.Attack;

        //ResetAllTriggers(animator);
        alreadyCalled = false;
    }

    public void setAlreadyCalled()
    {
        alreadyCalled = false;
    }

    void ResetAllTriggers(Animator animator)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
    }
}
