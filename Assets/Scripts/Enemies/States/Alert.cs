using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : StateBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5;

    private enum ChasingType
    {
        Terrestrial,
        Flying
    }

    [SerializeField]
    private ChasingType type;
    [SerializeField]
    private float chasingSpeed = 5;

    private Transform player;

    private PlayerDetection playerDetection;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        playerDetection = GetComponent<PlayerDetection>();
    }

    public override void Behaviour()
    {
        // Calcular dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        if (type == ChasingType.Terrestrial)
            direction = new Vector2(direction.x, 0);

        if (playerDetection.detectionType == PlayerDetection.DetectionType.Circle)
            Flip();

        // Aplicar movimiento al Rigidbody2D
        rb.velocity = direction * chasingSpeed;
    }

    private void Flip()
    {
        if (transform.position.x > player.position.x && spriteRenderer[0].flipX)
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
                spriteRenderer[i].flipX = false;
        }
        else if (transform.position.x < player.position.x && !spriteRenderer[0].flipX)
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
                spriteRenderer[i].flipX = true;
        }
    }
}
