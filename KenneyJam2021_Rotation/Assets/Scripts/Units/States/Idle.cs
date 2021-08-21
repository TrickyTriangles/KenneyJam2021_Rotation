using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;

namespace StateMachine.Player_Unit
{
    public class Idle : PlayerUnitBaseState
    {
        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            while (true)
            {
                yield return null;
            }
        }
    }
}
