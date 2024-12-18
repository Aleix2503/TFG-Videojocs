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
        //transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
