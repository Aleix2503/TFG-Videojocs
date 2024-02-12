using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public LayerMask disableWhenTouchingLayer;

    private PlayerValues playerValues;

    public void Initialize(PlayerValues playerValues)
    {
        this.playerValues = playerValues;
    }

    private void OnEnable()
    {
        if (playerValues == null) return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == disableWhenTouchingLayer)
        {
            this.enabled = false;
        }
    }
}
