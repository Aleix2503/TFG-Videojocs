using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : StateBehaviour
{
    [SerializeField]
    protected GameObject dieParticles;

    private new void Start()
    {
        base.Start();
    }

    public override void Behaviour()
    {
        Instantiate(dieParticles, transform.position, dieParticles.transform.rotation);
        transform.parent.gameObject.SetActive(false);
    }
}
