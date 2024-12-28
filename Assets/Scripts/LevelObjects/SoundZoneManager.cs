using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundZoneManager : MonoBehaviour
{
    public EventReference beachZone;
    public EventInstance beachZoneInstance;
    public EventReference caveZone;
    public EventInstance caveZoneInstance;

    public void Awake()
    {
        beachZoneInstance = RuntimeManager.CreateInstance(beachZone);
        caveZoneInstance = RuntimeManager.CreateInstance(caveZone);
    }
    public void Start()
    {
        caveZoneInstance.start();
    }
    public void OnDisable()
    {
        caveZoneInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
