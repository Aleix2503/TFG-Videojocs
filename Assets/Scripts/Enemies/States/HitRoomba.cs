using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRoomba : Hit
{
    [SerializeField]
    private float hitTime = 0.8f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Behaviour()
    {
        StartCoroutine(hit());
    }

    private IEnumerator hit()
    {
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(hitTime);

        fsmEnemies.state = FSMEnemies.State.Patrol;
    }
}
