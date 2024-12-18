using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatParticles : MonoBehaviour
{
    public GameObject splatPrefab;
    public Transform splatHolder;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem splatParticles;
    
    private int currentLayer = 4;

    private void Start()
    {
        splatParticles = GetComponent<ParticleSystem>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeColor(new Color(0.58f, 0.73f, 1f), new Color(0.18f, 0.49f, 1f));
            IncrementLayer();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeColor(new Color(1f, 0f, 1f), new Color(1f, 0.4f, 1f));
            IncrementLayer();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeColor(new Color(1f, 1f, 0f), new Color(1f, 0.8f, 0f));
            IncrementLayer();
        }
    }
    
    private void ChangeColor(Color color1, Color color2)
    {
        // Set the start color parameter to be random between the two specified colors
        var main = splatParticles.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color1, color2);
    }
    
    private void IncrementLayer()
    {
        // Increment the layer
        currentLayer++;
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(splatParticles, other, collisionEvents);
        int count = collisionEvents.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject go = PaintManager._instance.PlaceSplat(collisionEvents[i].intersection, collisionEvents[i].normal, splatParticles.main.startColor.color);
            if (other.CompareTag("Enemy"))
            {
                go.transform.parent = other.transform;
            }
        }
        
    }
}