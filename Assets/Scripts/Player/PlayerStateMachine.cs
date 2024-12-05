using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine: ScriptableObject
{
    public PlayerBehaviour currentBehaviour { get; private set; }
    public Animator _animator;
    public PlayerPhysics _playerPhysics;
    private PlayerController _playerController;

    public DashPlayerAbility dashPlayerAbility;
    public BubblePlayerAbility bubblePlayerAbility;
    public ExpandPlayerAbility expandPlayerAbility;

    public void Initialize(PlayerController playerController,Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
        _playerPhysics = playerController.playerPhysics;
        currentBehaviour = new IdlePlayerBehaviour(this, _playerPhysics, _playerController);
        currentBehaviour.Enter();


        dashPlayerAbility = new DashPlayerAbility(this, _playerPhysics, playerController,_playerController.abilityState);
        bubblePlayerAbility = new BubblePlayerAbility(this, _playerPhysics, playerController,_playerController.abilityState);
        expandPlayerAbility = new ExpandPlayerAbility(this, _playerPhysics, playerController,_playerController.abilityState);
    }

    public void ChangeState(PlayerBehaviour behaviour)
    {
        if(currentBehaviour is FallPlayerBehaviour)
        {
            SetAnim(1);
        }
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
            }
        }else if(num == 1)
        {
            _animator.SetTrigger("hasLanded");
        }
    }
}
