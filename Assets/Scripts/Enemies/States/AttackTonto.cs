using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTonto : Attack
{
    private Transform player; // Referencia al jugador

    [SerializeField]
    private float jumpHeight = 3f; // Altura del salto
    [SerializeField]
    private float jumpDistance = 5f; // Distancia horizontal del salto

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        fsmEnemies = GetComponent<FSMEnemies>();
    }

    public override void Behaviour()
    {
        // Calcular dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Obtener la posición objetivo a la distancia definida
        Vector2 targetPosition = (Vector2)transform.position + direction * jumpDistance;

        // Calcular la velocidad inicial necesaria para alcanzar el objetivo
        Vector2 jumpVelocity = CalculateJumpVelocity(transform.position, targetPosition, jumpHeight);

        // Aplicar la velocidad al Rigidbody2D
        rb.velocity = jumpVelocity;

        fsmEnemies.state = FSMEnemies.State.Recover;
    }

    Vector2 CalculateJumpVelocity(Vector2 start, Vector2 target, float height)
    {
        // Distancia horizontal entre el inicio y el objetivo
        float distance = target.x - start.x;

        // Altura máxima que debe alcanzar el salto
        float gravity = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;

        // Tiempo necesario para alcanzar el punto máximo
        float timeToApex = Mathf.Sqrt(2 * height / gravity);

        // Velocidad vertical inicial para alcanzar la altura deseada
        float verticalVelocity = gravity * timeToApex;

        // Tiempo total del salto (subida y bajada)
        float totalTime = timeToApex * 2;

        // Velocidad horizontal inicial para cubrir la distancia en el tiempo total
        float horizontalVelocity = distance / totalTime;

        return new Vector2(horizontalVelocity, verticalVelocity);
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            // Dibujar una línea hacia el jugador
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);

            // Dibujar el objetivo del salto
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 targetPosition = (Vector2)transform.position + direction * jumpDistance;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPosition, 0.2f);
        }
    }
}
