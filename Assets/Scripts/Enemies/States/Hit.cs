using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateBehaviour
{
    [Header("Hit Values")]
    [SerializeField]
    private int hitDamage = 2;
    [SerializeField]
    protected float hitTime = 0.5f;
    [Space]

    [Header("Recoil Values")]
    [SerializeField]
    private bool hasRecoil = false;
    [SerializeField]
    private float impulseStrength = 1.0f;

    protected bool alreadyHit = false;

    private Vector2 hitDirection = Vector2.zero;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public override void Behaviour()
    {
        if (!alreadyHit) StartCoroutine(hitTimer());
    }

    private IEnumerator hitTimer()
    {
        alreadyHit = true;
        fsmEnemies.life -= hitDamage;

        if (hasRecoil) rb.AddForce(hitDirection * impulseStrength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(hitTime);

        alreadyHit = false;
        if (GetComponent<Patrol>() != null)
            fsmEnemies.state = FSMEnemies.State.Patrol;
        else if (GetComponent<Idle>() != null)
            fsmEnemies.state = FSMEnemies.State.Idle;
        else
            fsmEnemies.state = FSMEnemies.State.Attack;
    }

    public void hitDirectionVector(Vector2 direction)
    {
        hitDirection = direction;
    }
}
