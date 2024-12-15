using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateBehaviour
{
    [SerializeField]
    private int hitDamage = 2;
    [SerializeField]
    protected float hitTime = 0.5f;

    private FSMEnemies fSMEnemies;
    protected bool alreadyHit = false;

    // Start is called before the first frame update
    void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
    }

    public override void Behaviour()
    {
        if (!alreadyHit) StartCoroutine(hitTimer());
    }

    private IEnumerator hitTimer()
    {
        alreadyHit = true;
        fsmEnemies.life -= hitDamage;

        yield return new WaitForSeconds(hitTime);

        alreadyHit = false;
        if (GetComponent<Patrol>() != null)
            fsmEnemies.state = FSMEnemies.State.Patrol;
        else if (GetComponent<Idle>() != null)
            fsmEnemies.state = FSMEnemies.State.Idle;
        else
            fsmEnemies.state = FSMEnemies.State.Attack;
    }
}
