using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Enter");
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().SetRespawnPosition(transform.position);
        }
    }
}
