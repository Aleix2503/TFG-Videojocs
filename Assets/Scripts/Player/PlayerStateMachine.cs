using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(PlayerState state)
    {
        currentState.Exit();

        currentState = state;

        currentState.Enter();
        
        UpdatePaintingState(state);
    }

    private void UpdatePaintingState(PlayerState state)
    {
        if (state is PlayerMoveState)
        {
            PaintManager._instance.SetPaintingStateToMoving();
        }
        else if (state is PlayerDashState)
        {
            PaintManager._instance.SetPaintingStateToDashing();
        }
        else if (state is PlayerExpandedState)
        {
            PaintManager._instance.SetPaintingStateToExpanded();
        }
        else PaintManager._instance.SetPaintingStateToDefault();
    }
}
