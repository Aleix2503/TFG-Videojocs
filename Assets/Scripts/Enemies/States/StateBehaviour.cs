using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour : MonoBehaviour
{
    protected SpriteRenderer[] spriteRenderer;
    protected FSMEnemies fsmEnemies;
    protected Animator animator;
    protected Rigidbody2D rb;

    protected void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
    }

    public abstract void Behaviour();
}
