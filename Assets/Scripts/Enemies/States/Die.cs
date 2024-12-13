using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : StateBehaviour
{
    public override void Behaviour()
    {
        Destroy(transform.parent.gameObject);
    }
}
