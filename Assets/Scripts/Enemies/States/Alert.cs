using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : StateBehaviour
{
    private enum ChasingType
    {
        Terrestrial,
        Flying
    }

    [SerializeField]
    private ChasingType type;
    [SerializeField]
    private float chasingSpeed = 5;

    private Rigidbody2D rb;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        fsmEnemies = GetComponent<FSMEnemies>();
    }

    public override void Behaviour()
    {
        // Calcular dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        if (type == ChasingType.Terrestrial)
        {
            direction = new Vector2(direction.x, 0);
        }

        // Aplicar movimiento al Rigidbody2D
        rb.velocity = direction * chasingSpeed;

        // Ajustar la orientación del enemigo
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1); // Mirar hacia el jugador
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && fsmEnemies.state == FSMEnemies.State.Alert)
        {
            fsmEnemies.state = FSMEnemies.State.Attack; 
        }
    }
}
