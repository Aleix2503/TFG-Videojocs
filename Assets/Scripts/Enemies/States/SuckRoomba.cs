using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuckRoomba : MonoBehaviour
{
    private FSMEnemies fSMEnemies;

    [SerializeField]
    private float detectionRadius = 5f; // Radio del círculo
    [SerializeField]
    private Vector2 offset = new Vector2(0, -1); // Offset hacia abajo desde el centro

    [SerializeField]
    private float suckingVelocity = 1.0f;

    [SerializeField]
    private float suckingTimer = 1.0f;
    private bool sucking = false;

    [SerializeField]
    private float suckAgainTimer = 0.8f;
    private bool canSuck = true;

    private Patrol patrol;
    private float previousPatrolSpeed;

    // Start is called before the first frame update
    void Start()
    {
        fSMEnemies = GetComponent<FSMEnemies>();
        patrol = GetComponent<Patrol>();
        previousPatrolSpeed = patrol.patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (fSMEnemies.state == FSMEnemies.State.Patrol && !sucking && canSuck)
        {
            suck();
            StartCoroutine(canSuckTimer());
        }
    }

    private IEnumerator canSuckTimer ()
    {
        canSuck = false;
        yield return new WaitForSeconds(suckAgainTimer);
        canSuck = true;
    }

    private IEnumerator roombaSucking ()
    {
        sucking = true;
        patrol.patrolSpeed = suckingVelocity;

        yield return new WaitForSeconds(suckingTimer);

        sucking = false;
        patrol.patrolSpeed = previousPatrolSpeed;
    }

    private void suck()
    {
        GameObject[] raycastHit = PerformWideRaycast();

        if (raycastHit.Length > 0)
        {
            if (!sucking)
            {
                StartCoroutine(roombaSucking());
            }

            foreach (var splatter in raycastHit)
            {
                Destroy(splatter.gameObject);

                if (fSMEnemies.life <= 0)
                    fSMEnemies.state = FSMEnemies.State.Die;
                else
                    fSMEnemies.life--;
            }
                
        }
    }

    private GameObject[] PerformWideRaycast()
    {
        // Calcular el centro del círculo con el offset
        Vector2 detectionCenter = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)offset);

        // Obtener todos los colliders dentro del radio
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionCenter, detectionRadius);

        // Filtrar los objetos con el tag especificado
        var detectedObjects = new List<GameObject>();

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Splatter"))
            {
                detectedObjects.Add(hit.gameObject);
            }
        }

        // Convertir la lista en un array y devolverlo
        return detectedObjects.ToArray();
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el círculo en el editor
        Gizmos.color = Color.red;

        // Calcular el centro del círculo con el offset
        Vector2 detectionCenter = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)offset);

        // Dibujar el círculo
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);
    }
}
