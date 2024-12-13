using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateBehaviour
{
    [SerializeField]
    private int hitDamage = 2;

    private FSMEnemies fSMEnemies;

    // Start is called before the first frame update
    void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
    }

    public override void Behaviour()
    {
        fsmEnemies.life -= hitDamage;
        fsmEnemies.state = GetComponent<Idle>() != null ? FSMEnemies.State.Idle : FSMEnemies.State.Patrol; 
    }
}
