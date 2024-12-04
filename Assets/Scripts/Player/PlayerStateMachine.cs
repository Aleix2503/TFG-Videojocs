using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine: ScriptableObject
{
    public PlayerBehaviour currentBehaviour { get; private set; }
    private Animator _animator;
    public PlayerPhysics _playerPhysics;
    private PlayerController _playerController;

    public void Initialize(PlayerController playerController,Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
        _playerPhysics = playerController.playerPhysics;
        currentBehaviour = new IdlePlayerBehaviour(this, _playerPhysics, _playerController);
        currentBehaviour.Enter();
    }

    public void ChangeState(PlayerBehaviour behaviour)
    {
        currentBehaviour = behaviour;
        currentBehaviour.Enter();
        SetAnim();
    }
    public void SetAnim()
    {
        switch(currentBehaviour){
            case IdlePlayerBehaviour:
                _animator.Play("Idle");
                break;
            case MovePlayerBehaviour:
                _animator.Play("Move");
                break;
        }
    }
}
