using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemies : MonoBehaviour
{
    protected enum State
    {
        Patrol,
        Alert,
        Attack,
        Recover,
        Hit,
        Die
    }

    [SerializeField]
    protected State state;

    void Start()
    {
        state = State.Patrol;
    }

    void Update()
    {
        StateBehaviour behaviour = GetComponent(state.ToString()) as StateBehaviour;
        behaviour.Behaviour();
    }
}
