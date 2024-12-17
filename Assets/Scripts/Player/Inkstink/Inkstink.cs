using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inkstink : MonoBehaviour
{
    public PlayerPhysics playerPhysics;

    public List<Material> inkstinctMaterials;

    private void OnInkstinkIn()
    {
        if (playerPhysics.isGrounded)
        {
            playerPhysics.isInkstink = true;
            foreach (Material material in inkstinctMaterials)
            {
                material.SetFloat("_isActive", 1);
            }
        }
    }
    private void OnInkstinkOut()
    {
        playerPhysics.isInkstink = false;
        foreach (Material material in inkstinctMaterials)
        {
            material.SetFloat("_isActive", 0);
        }
    }
    private void FixedUpdate()
    {
        if (!playerPhysics.isGrounded)
        {
            OnInkstinkOut();
        }
    }
}
