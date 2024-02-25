using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickupBehavior : MonoBehaviour
{
    [SerializeField]
    PlayerController.AbilityType abilityToUnlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.UnlockAbility(abilityToUnlock);

            Destroy(gameObject);
        }
    }
}
