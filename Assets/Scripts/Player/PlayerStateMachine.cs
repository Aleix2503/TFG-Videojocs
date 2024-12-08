using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : ScriptableObject
{
    public PlayerBehaviour currentBehaviour { get; private set; }
    public Animator _animator;
    public PlayerPhysics _playerPhysics;
    private PlayerController _playerController;

    public void Initialize(PlayerController playerController, Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
        _playerPhysics = playerController.playerPhysics;
        currentBehaviour = new IdlePlayerBehaviour(this, _playerPhysics, _playerController);
        currentBehaviour.Enter();
    }

    public void ChangeState(PlayerBehaviour behaviour)
    {
        _playerController.SetPaintingState(PlayerPaintingState.def);
        currentBehaviour = behaviour;
        currentBehaviour.Enter();
        SetAnim(0);
    }
    public void SetAnim(int num)
    {
        if (num == 0)
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
                    _animator.SetTrigger("isFalling");
                    break;
                case DashPlayerBehaviour:
                    _animator.SetTrigger("isDashing");
                    break;
                case BubblePlayerBehaviour:
                    _animator.SetBool("isBubbling", true);
                    break;
                case ExpandPlayerBehaviour:
                    _animator.SetTrigger("isExpanding");
                    break;
            }
        }
        else if (num == 1)
        {
            _animator.SetTrigger("hasLanded");
        }
        else if (num == 2)
        {
            _animator.SetBool("isBubbling", false);
        }
        }
}
