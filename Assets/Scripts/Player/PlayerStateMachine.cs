using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine: MonoBehaviour
{
    public PlayerBehaviour currentBehaviour { get; private set; }
    private Animator _animator;
    private PlayerPhysics _playerPhysics;
    private PlayerController _playerController;

    public void Initialize()
    {
        _playerPhysics = GetComponent<PlayerPhysics>();
        _playerController = GetComponent<PlayerController>();
        currentBehaviour = new IdlePlayerBehaviour(this,_playerPhysics,_playerController);
        currentBehaviour.Enter();
        _animator = GetComponent<Animator>();
    }

    public void ChangeState(PlayerBehaviour behaviour)
    {
        currentBehaviour = behaviour;
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
