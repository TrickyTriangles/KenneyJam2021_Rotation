using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;

public class PlayerUnit : MonoBehaviour
{
    private StateMachine.StateMachine<PlayerUnitBaseState> state_machine;
    [SerializeField] private UnitProfile profile;

    private void Start()
    {
        state_machine = new StateMachine.StateMachine<PlayerUnitBaseState>(this, new PlayerUnitBaseState());
        state_machine.SetNextState(new Move(profile.velocity));

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Subscribe_GameEnd(GameManager_GameEnd);
        }
    }

    private void GameManager_GameEnd()
    {
        state_machine.SetNextState(new Idle());
    }

    public void MoveUnit(Task task)
    {
        state_machine.SetNextState(new Move(profile.velocity));
    }
}