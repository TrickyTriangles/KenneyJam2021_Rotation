using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;

namespace StateMachine.Player_Unit
{
    public class Move : PlayerUnitBaseState
    {
        private float velocity;

        public Move(float _velocity)
        {
            velocity = _velocity;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            while (true)
            {
                subject.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, velocity * Time.deltaTime));

                yield return null;
            }
        }
    }
}
