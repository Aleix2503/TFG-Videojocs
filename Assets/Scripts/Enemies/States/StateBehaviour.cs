using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour : MonoBehaviour
{
    protected FSMEnemies fsmEnemies;

    private void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
    }
    public abstract void Behaviour();
}
