using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;

namespace StateMachine.Player_Unit
{
    public class Move : PlayerUnitBaseState
    {
        private Task destination;
        private float duration = 2f;

        public Move(Task _destination)
        {
            destination = _destination;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            float timer = 0f;
            Vector3 start_pos = subject.transform.position;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                subject.transform.position = Vector3.Slerp(start_pos, destination.transform.position, MathUtils.SmootherStep(0f, duration, timer));

                yield return null;
            }

            next_state_callback?.Invoke(new Idle());
        }
    }
}
