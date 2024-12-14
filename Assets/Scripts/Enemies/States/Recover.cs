using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover : StateBehaviour
{
    [SerializeField]
    private float timeRecover = 3;

    private bool alreadyCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
    }

    public override void Behaviour()
    {
        if (!alreadyCalled)
            StartCoroutine(jumpRecover());
    }

    private IEnumerator jumpRecover()
    {
        alreadyCalled = true;

        yield return new WaitForSeconds(timeRecover);

        if (GetComponent<Patrol>() != null)
            fsmEnemies.state = FSMEnemies.State.Patrol;
        else if (GetComponent<Idle>() != null)
        {
            fsmEnemies.state = FSMEnemies.State.Idle;
            GetComponent<Idle>().SetAlreadyArrivedFalse();
        } 
        else fsmEnemies.state = FSMEnemies.State.Attack;

        alreadyCalled = false;
    }
}
