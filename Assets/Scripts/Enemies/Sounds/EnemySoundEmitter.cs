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
    private EventInstance absorbRoomba;
    private EventInstance dieSound;
    private EventInstance alertTonto;
    private EventInstance chaseMosca;
    private EventInstance dashMiniBoss;

    private void Start()
    {
        idleRoomba = RuntimeManager.CreateInstance(enemiesSoundReferences.idleRoomba);
        patrolMosca = RuntimeManager.CreateInstance(enemiesSoundReferences.patrolMosca);
        idleTonto = RuntimeManager.CreateInstance(enemiesSoundReferences.idleTonto);
        absorbRoomba = RuntimeManager.CreateInstance(enemiesSoundReferences.absorbRoomba);
        dieSound = RuntimeManager.CreateInstance(enemiesSoundReferences.dieEnemy);
        alertTonto = RuntimeManager.CreateInstance(enemiesSoundReferences.alertTonto);
        chaseMosca = RuntimeManager.CreateInstance(enemiesSoundReferences.chaseMosca);
        dashMiniBoss = RuntimeManager.CreateInstance(enemiesSoundReferences.dashMiniBoss);


    }
    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(idleRoomba, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(patrolMosca, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(idleTonto, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(absorbRoomba, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(dieSound, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(alertTonto, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(chaseMosca, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(dashMiniBoss, transform, GetComponent<Rigidbody>());
    }
    private void OnDisable()
    {
        idleRoomba.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        patrolMosca.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        idleTonto.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        absorbRoomba.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dieSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        alertTonto.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        chaseMosca.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dashMiniBoss.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void PlayDieEnemy()
    {
        dieSound.start();
        StopIdleRoomba();
        StopPatrolMosca();
        StopIdleTonto();
    }
    public void PlayAbsorbRoomba()
    {
        StopIdleRoomba();
        absorbRoomba.start();
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
        alertTonto.start();
    }
    public void PlayChaseMosca()
    {
        StopPatrolMosca();
        chaseMosca.start();
    }
    public void PlayDashMiniBoss()
    {
        dashMiniBoss.start();
    }
}
