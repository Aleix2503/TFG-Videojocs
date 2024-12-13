using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PatrolRoomba : Patrol
{
    private enum SetPoint
    {
        UpLeft,
        DownLeft,
        UpRight,
        DownRight
    }

    [SerializeField]
    private float rotationSpeed = 5;

    private float height;
    private float width;

    private float offsetHeight;
    private float offsetWidth;

    [Header("AssignPoints")]
    [SerializeField]
    private float offset = 0.05f;
    [SerializeField]
    private TileBase tile;
    [SerializeField]
    private SetPoint setPoint;
    [SerializeField]
    private int pointNumber;
    [Space]

    [Header("TileSelector")]
    [SerializeField]
    public Tilemap tilemap; // Asignar el Tilemap
    public Vector3Int selectedTilePosition; // Posición de la tile seleccionada

    private bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points have been assigned");
        }
    }

    public override void Behaviour()
    {
        if (patrolPoints.Count == 0)
            return;

        MoveToPoint();
    }

    private void MoveToPoint()
    {
        Vector3 targetPosition = patrolPoints[currentPointIndex].transform.position;

        Vector2 direction = (targetPosition - transform.position).normalized;
        Vector2 movement = direction * patrolSpeed;

        rb.velocity = movement;


        if (Vector2.Distance(transform.position, targetPosition) < 0.1f && !rotating)
        {
            StartCoroutine(RotateSmoothly());
        }

        if (Vector2.Distance(transform.position, targetPosition) < 0.005f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            rotating = false;
        }
    }

    public void SaveSelectedTile(Vector3Int position)
    {
        selectedTilePosition = position;
        Debug.Log($"Tile seleccionada en posición: {selectedTilePosition}");

        tile = tilemap.GetTile(position);
    }

    public void SetPatrolPoints()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 worldPosition = tilemap.CellToWorld(selectedTilePosition);

        height = spriteRenderer.bounds.extents.y;
        width = spriteRenderer.bounds.extents.x;

        offsetHeight = height + offset;
        offsetWidth = width + offset;

        switch (setPoint)
        {
            case (SetPoint.UpLeft):
                patrolPoints[pointNumber].transform.position = new Vector2(worldPosition.x - offsetWidth, worldPosition.y + tilemap.cellSize.y + offsetHeight);
                break;
            case (SetPoint.UpRight):
                patrolPoints[pointNumber].transform.position = new Vector2(worldPosition.x + tilemap.cellSize.x + offsetWidth, worldPosition.y + tilemap.cellSize.y + offsetHeight);
                break;
            case (SetPoint.DownLeft):
                patrolPoints[pointNumber].transform.position = new Vector2(worldPosition.x - offsetWidth, worldPosition.y - offsetHeight);
                break;
            case (SetPoint.DownRight):
                patrolPoints[pointNumber].transform.position = new Vector2(worldPosition.x + tilemap.cellSize.x + offsetWidth, worldPosition.y - offsetHeight);
                break;
        }
    }

    private IEnumerator RotateSmoothly()
    {
        rotating = true;

        Quaternion startRotation = transform.rotation; // Rotación inicial
        Quaternion endRotation = transform.rotation * Quaternion.Euler(new Vector3(0, 0, -90)); // Rotación final

        float totalAngle = Quaternion.Angle(startRotation, endRotation); // Ángulo total de rotación
        float duration = 0;

        if (patrolSpeed < 2)
            duration = totalAngle / (rotationSpeed / 2); // Tiempo requerido para completar la rotación
        else 
            duration = totalAngle / rotationSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolar rotación con base en el tiempo transcurrido
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime; // Incrementar el tiempo transcurrido
            yield return null; // Esperar al siguiente frame
        }

        // Asegurarnos de que la rotación final sea exacta
        transform.rotation = endRotation;
    }
}
