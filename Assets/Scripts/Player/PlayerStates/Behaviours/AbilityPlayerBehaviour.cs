using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlayerBehaviour : PlayerBehaviour
{
    private PlayerAbility currentAbility = null;
    private Animator _animator;
    public void AbilityIn(PlayerAbility ability)
    {
        currentAbility = ability;
        currentAbility.Activate();
    }
    public void AbilityOut()
    {
        currentAbility.Deactivate();
        currentAbility = null;
        _playerStateMachine.ChangeState(_playerController.idleState);
    }
    public void SetAnim()
    {
        switch (currentAbility)
        {
            case DashPlayerAbility:
                _animator.SetTrigger("isDashing");
                break;
            case BubblePlayerAbility:
                _animator.SetTrigger("isBubbling");
                break;
            case ExpandPlayerAbility:
                _animator.SetTrigger("isExpanding");
                break;
        }
    }
    public AbilityPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
        _animator = playerStateMachine._animator;
    }
    public override void FixedUpdate()
    {
        if (currentAbility == null)
        {
            if (_playerController.canDash)
            {
                AbilityIn(_playerStateMachine.dashPlayerAbility);
                _playerController.CheckIfCanDash();
                return;
            }
            else if (_playerController.canBubble)
            {
                AbilityIn(_playerStateMachine.bubblePlayerAbility);
                _playerController.CheckIfCanBubble();
                return;
            }
            else if (_playerController.canExpand)
            {
                AbilityIn(_playerStateMachine.expandPlayerAbility);
                _playerController.CheckIfCanExpand();
                return;
            }
        }
        
    }
}
