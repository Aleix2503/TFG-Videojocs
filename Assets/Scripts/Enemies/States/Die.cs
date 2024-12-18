using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : StateBehaviour
{
    private new void Start()
    {
        base.Start();
    }

    public override void Behaviour()
    {
        gameObject.SetActive(false);
    }
}
