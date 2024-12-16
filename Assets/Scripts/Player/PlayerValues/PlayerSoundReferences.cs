
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "newPlayerSoundReferences", menuName = "PlayerSoundReferences")]
public class PlayerSoundReferences : ScriptableObject
{   
    [Header("Player")]
    public EventReference jumpSound;

    public EventReference dashSound;

    public EventReference bubbleSound;

    public EventReference cubedSound;

    public EventReference deathSound;

    public EventReference hitAttack;

    public EventReference missAttack;

    public EventReference inkstinctSound;

    public EventReference landSound;

    public EventReference bounceBubbleSound;
}
