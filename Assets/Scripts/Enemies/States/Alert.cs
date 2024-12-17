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
        /*else if (type == ChasingType.Flying)
            RotateTowardsPlayer();*/

        if (playerDetection.detectionType == PlayerDetection.DetectionType.Circle)
            Flip();

        // Aplicar movimiento al Rigidbody2D
        rb.velocity = direction * chasingSpeed;

        /*// Ajustar la orientación del enemigo
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1); // Mirar hacia el jugador
        }*/
    }

    private void Flip()
    {
        if (transform.position.x > player.position.x && spriteRenderer[0].flipX)
        {
            Debug.Log("aqui");
            for (int i = 0; i < spriteRenderer.Length; i++)
                spriteRenderer[i].flipX = false;
        }
        else if (transform.position.x < player.position.x && !spriteRenderer[0].flipX)
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
                spriteRenderer[i].flipX = true;
        }
    }

    void RotateTowardsPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector2 direction = player.position - transform.position;

        // Calcular el ángulo en radianes y convertirlo a grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Crear una rotación en Z hacia el ángulo calculado
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Suavizar la rotación con Lerp o Slerp
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
