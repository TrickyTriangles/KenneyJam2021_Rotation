using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;
using StateMachine;

public class Flip : PlayerUnitBaseState
{
    private PlayerUnit.Direction current_direction;

    public Flip(PlayerUnit.Direction _current_direction)
    {
        current_direction = _current_direction;
    }

    public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
    {
        PlayerUnit unit = subject as PlayerUnit;
        unit.anim.Play("Flip", 0);
        yield return null;

        AnimatorStateInfo info = unit.anim.GetCurrentAnimatorStateInfo(0);
        float timer = 0f;

        while (timer < info.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        switch (current_direction)
        {
            case PlayerUnit.Direction.LEFT:
                next_state_callback(new Move(PlayerUnit.Direction.RIGHT));
                break;
            case PlayerUnit.Direction.RIGHT:
                next_state_callback(new Move(PlayerUnit.Direction.LEFT));
                break;
            default:
                break;
        }
    }
}
