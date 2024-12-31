using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickupBehavior : MonoBehaviour
{
    [SerializeField]
    PlayerController.AbilityType abilityToUnlock;
    public Collider2D collider2D;
    public Animator animator;
    public Rigidbody2D rb2D;

    public Color explosionColor;

    public float upwardsForce = 5f;
    public float deletionDelay = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.UnlockAbility(abilityToUnlock);

            animator.SetTrigger("PickUpTrigger");
            collider2D.enabled = false;

            AnimationMovement();
        }
    }

    private void AnimationMovement()
    {
        rb2D.bodyType = RigidbodyType2D.Dynamic;

        rb2D.AddForce(new Vector2(0, upwardsForce), ForceMode2D.Impulse);

        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        PaintManager._instance.InstanceExplosion(transform.position, abilityToUnlock);
    }
}
