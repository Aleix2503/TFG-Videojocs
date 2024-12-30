using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inkstink : MonoBehaviour
{
    public PlayerPhysics playerPhysics;

    public List<Material> inkstinctMaterials;
    public EventReference inkstinkSound;
    public EventInstance inkstinkInstance;

    public void Start()
    {
        inkstinkInstance = RuntimeManager.CreateInstance(inkstinkSound);
    }
    private void OnInkstinkIn()
    {
        if (playerPhysics.isGrounded)
        {
            playerPhysics.isInkstink = true;
            foreach (Material material in inkstinctMaterials)
            {
                material.SetFloat("_isActive", 1);
            }
            inkstinkInstance.start();
        }
    }
    private void OnInkstinkOut()
    {
        playerPhysics.isInkstink = false;
        foreach (Material material in inkstinctMaterials)
        {
            material.SetFloat("_isActive", 0);
        }
        inkstinkInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    private void FixedUpdate()
    {
        if (!playerPhysics.isGrounded)
        {
            OnInkstinkOut();
        }
    }
}
