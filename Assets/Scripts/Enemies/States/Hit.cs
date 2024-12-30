using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    [Header("Hit Values")]
    [SerializeField]
    private int hitDamage = 2;
    [SerializeField]
    protected float hitTime = 0.5f;
    [Space]

    [Header("Recoil Values")]
    [SerializeField]
    private bool hasRecoil = false;
    [SerializeField]
    private float impulseStrength = 1.0f;

    private Vector2 hitDirection = Vector2.zero;

    protected FSMEnemies fsmEnemies;
    protected Rigidbody2D rb;
    protected Animator animator;

    // Start is called before the first frame update
    protected void Start()
    {
        fsmEnemies = GetComponent<FSMEnemies>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    protected virtual void Behaviour()
    {
        fsmEnemies.life -= hitDamage;
        if (hasRecoil) rb.AddForce(hitDirection * impulseStrength, ForceMode2D.Impulse);
    }

    public void hitDirectionVector(Vector2 direction)
    {
        hitDirection = direction;

        Behaviour();
    }
}
