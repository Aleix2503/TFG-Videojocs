using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : StateBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Behaviour()
    {
        Debug.Log("Jugador detectado en el cono de visión");
    }
}
