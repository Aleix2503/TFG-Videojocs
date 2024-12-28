using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEmitter : MonoBehaviour
{
    public EnemiesSoundReferences enemiesSoundReferences;

    public enum EnemyType
    {
        Tonto,
        Mosca,
        Roomba,
        MiniBoss
    }
    public EnemyType enemyType;

    private EventInstance idleRoomba;
    private EventInstance patrolMosca;
    private EventInstance idleTonto;

    private void Start()
    {
        idleRoomba = RuntimeManager.CreateInstance(enemiesSoundReferences.idleRoomba);
        patrolMosca = RuntimeManager.CreateInstance(enemiesSoundReferences.patrolMosca);
        idleTonto = RuntimeManager.CreateInstance(enemiesSoundReferences.idleTonto);
    }
    public void PlayDieEnemy()
    {
        RuntimeManager.PlayOneShot(enemiesSoundReferences.dieEnemy);
        StopIdleRoomba();
        StopPatrolMosca();
        StopIdleTonto();
    }
    public void PlayAbsorbRoomba()
    {
        StopIdleRoomba();
        RuntimeManager.PlayOneShot(enemiesSoundReferences.absorbRoomba);
    }
    public void PlayIdleRoomba()
    {
        idleRoomba.start();
    }
    public void StopIdleRoomba()
    {
        idleRoomba.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void PlayPatrolMosca()
    {
        patrolMosca.start();
    }
    public void StopPatrolMosca()
    {
        patrolMosca.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void PlayIdleTonto()
    {
        idleTonto.start();
    }
    public void StopIdleTonto()
    {
        idleTonto.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void PlayAlertTonto()
    {
        StopIdleTonto();
        RuntimeManager.PlayOneShot(enemiesSoundReferences.alertTonto);
    }
    public void PlayChaseMosca()
    {
        StopPatrolMosca();
        RuntimeManager.PlayOneShot(enemiesSoundReferences.chaseMosca);
    }
    public void PlayDashMiniBoss()
    {
        RuntimeManager.PlayOneShot(enemiesSoundReferences.dashMiniBoss);
    }
}
