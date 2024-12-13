using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [Header("Hitbox")]
    [SerializeField]
    private GameObject hitbox;
    [SerializeField]
    private Vector2[] posicions;
    [Space]

    [Header("Cooldowns")]
    [SerializeField]
    private float cooldownAttack;
    [Space]

    [Header("Durations")]
    [SerializeField]
    private float durationNormalAttack;
    [Space]

    [Header("Animator")]
    [SerializeField]
    private Animator animator;

    [HideInInspector]
    public bool isAttacking = false;
    private bool attackedFirst = false;
    private float attackTimer = 0f;

    private PlayerController playerController;

    private Type[] canAttackBehaviours = { typeof(IdlePlayerBehaviour), typeof(MovePlayerBehaviour), typeof(AirPlayerBehaviour) };

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void OnAttack()
    {
        if (Array.Exists(canAttackBehaviours, type => type.IsInstanceOfType(playerController.stateMachine.currentBehaviour)) && !isAttacking)
        {
            if (attackTimer < cooldownAttack && attackedFirst)
            {
                animator.SetTrigger("isAttacking2");
                attackedFirst = false;
                StartCoroutine(hitboxActive(durationNormalAttack, 0));
            }
            else if (attackTimer >= cooldownAttack)
            {
                animator.SetTrigger("isAttacking");
                attackedFirst = true;
                StartCoroutine(hitboxActive(durationNormalAttack, 0));
            }
        }
    }

    private IEnumerator hitboxActive(float seconds, int posicio)
    {
        hitbox.SetActive(true);
        hitbox.transform.localPosition = new Vector3(posicions[posicio].x, posicions[posicio].y);
        isAttacking = true;

        yield return new WaitForSecondsRealtime(seconds);

        hitbox.SetActive(false);
        hitbox.transform.localPosition = Vector3.zero;
        isAttacking = false;
        attackTimer = 0f;
    }
}
