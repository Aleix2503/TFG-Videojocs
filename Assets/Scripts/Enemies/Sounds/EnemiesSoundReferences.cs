using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesSoundReferences", menuName = "EnemiesSoundReferences")]
public class EnemiesSoundReferences : ScriptableObject
{
    [Header("General")]
    public EventReference dieEnemy;
    [Space()]
    [Header("Tonto")]
    public EventReference alertTonto;
    public EventReference idleTonto;
    [Space()]
    [Header("Mosca")]
    public EventReference patrolMosca;
    public EventReference chaseMosca;
    [Space()]
    [Header("Roomba")]
    public EventReference idleRoomba;
    public EventReference absorbRoomba;
    [Space()]
    [Header("Mini boss")]
    public EventReference dashMiniBoss;


}
