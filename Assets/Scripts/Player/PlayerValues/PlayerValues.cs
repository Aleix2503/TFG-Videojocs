using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerValues", menuName = "PlayerValues")]
public class PlayerValues : ScriptableObject
{
    [Header("Ability Unlocks")]
    public bool isAbility1Unlocked = false;
    public bool isAbility2Unlocked = false;
    public bool isAbility3Unlocked = false;

    [Header("Move State")]
    public float moveSpeed = 10f;
}
