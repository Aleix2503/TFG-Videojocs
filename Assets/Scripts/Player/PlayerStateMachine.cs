using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : ScriptableObject
{
    public PlayerBehaviour currentBehaviour { get; private set; }
    public Animator _animator;
    public PlayerPhysics _playerPhysics;
    private PlayerController _playerController;
    private PlayerSoundReferences _playerSoundReferences;

    public EventInstance inkstink;

    public void Initialize(PlayerController playerController, Animator animator, PlayerSoundReferences playerSoundReferences)
    {
        _playerController = playerController;
        _animator = animator;
        _playerPhysics = playerController.playerPhysics;
        _playerSoundReferences = playerSoundReferences;
        currentBehaviour = new IdlePlayerBehaviour(this, _playerPhysics, _playerController);
        currentBehaviour.Enter();

        inkstink = RuntimeManager.CreateInstance(_playerSoundReferences.inkstinctSound);
    }

    public void ChangeState(PlayerBehaviour behaviour)
    {
        if(currentBehaviour is FallPlayerBehaviour&&behaviour!=currentBehaviour)
        {
            _animator.SetBool("isFalling", false);
        }
        _playerController.SetPaintingState(PlayerPaintingState.def);
        currentBehaviour = behaviour;
        currentBehaviour.Enter();
        SetAnim();
        SetSoundState();
    }
    public void SetAnimBool(string name, bool mode)
    {
        _animator.SetBool(name, mode);
    }
    public void SetAnimTrigger(string name)
    {
        _animator.SetTrigger(name);
    }
    public void SetAnim()
    {
        switch (currentBehaviour)
        {
            case IdlePlayerBehaviour:
                _animator.SetBool("isMoving", false);
                break;
            case MovePlayerBehaviour:
                _animator.SetBool("isMoving", true);
                break;
            case JumpPlayerBehaviour:
                _animator.SetTrigger("isJumping");
                break;
            case FallPlayerBehaviour:
                if (!_playerController.isCoyoteTimeActive) { _animator.SetTrigger("isFalling"); }
                break;
            case DashPlayerBehaviour:
                _animator.SetTrigger("isDashing");
                break;
            case BubblePlayerBehaviour:
                _animator.SetBool("isBubbling", true);
                break;
            case CubedPlayerBehaviour:
                _animator.SetTrigger("isCubing");
                break;
            case DeathPlayerBehaviour:
                _animator.SetTrigger("isDying");
                break;
        }
    }
    public void SetSoundState()
    {
        switch (currentBehaviour)
        {
            case JumpPlayerBehaviour:
                RuntimeManager.PlayOneShot(_playerSoundReferences.jumpSound);
                break;
            case DashPlayerBehaviour:
                RuntimeManager.PlayOneShot(_playerSoundReferences.dashSound);
                break;
            case BubblePlayerBehaviour:
                RuntimeManager.PlayOneShot(_playerSoundReferences.bubbleSound);
                break;
            case DeathPlayerBehaviour:
                RuntimeManager.PlayOneShot(_playerSoundReferences.deathSound);
                break;
        }
    }
    public void SetSound(string name)
    {
        switch (name)
        {
            case "bounceBubble":
                RuntimeManager.PlayOneShot(_playerSoundReferences.bounceBubbleSound);
                break;
            case "land":
                RuntimeManager.PlayOneShot(_playerSoundReferences.landSound);
                break;
            case "bubble":
                RuntimeManager.PlayOneShot(_playerSoundReferences.bubbleSound);
                break;
            case "cube":
                RuntimeManager.PlayOneShot(_playerSoundReferences.cubedSound);
                break;
        }
    }
    public void SetSoundMode(string name, bool mode)
    {
        if (mode)
        {
            inkstink.start();
        }
        else
        {
            inkstink.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
