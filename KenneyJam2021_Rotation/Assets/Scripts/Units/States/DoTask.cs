using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;
using StateMachine;

public class DoTask : PlayerUnitBaseState
{
    Task task;

    public DoTask(Task _task)
    {
        task = _task;
    }

    public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
    {
        next_state_callback(new Idle());
        yield return null;
    }
}
