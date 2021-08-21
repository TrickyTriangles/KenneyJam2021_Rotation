using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;

public class PlayerUnit : MonoBehaviour
{
    private StateMachine.StateMachine<PlayerUnitBaseState> state_machine;

    private void Start()
    {
        state_machine = new StateMachine.StateMachine<PlayerUnitBaseState>(this, new PlayerUnitBaseState());
        state_machine.SetNextState(new Idle());
    }

    public void MoveUnit(Task task)
    {
        state_machine.SetNextState(new Move(task));
    }
}