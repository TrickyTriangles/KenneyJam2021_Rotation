using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;

namespace StateMachine.Player_Unit
{
    public class Move : PlayerUnitBaseState
    {
        PlayerUnit.Direction direction;
        PlayerUnit unit;

        public Move(PlayerUnit.Direction _direction)
        {
            direction = _direction;
        }

        private void PlayerUnit_UnitClicked(object sender, EventArgs args)
        {
            EndOfStateCleanup();
            unit.ChangeState(new Flip(direction));
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            unit = subject as PlayerUnit;
            unit.Subscribe_UnitClicked(PlayerUnit_UnitClicked);

            unit.SetDirection(direction);
            unit.anim.Play("Move", 0);

            float move_dir = direction == PlayerUnit.Direction.LEFT ? 1f : -1f;

            while (true)
            {
                subject.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, unit.velocity * move_dir * Time.deltaTime));

                yield return null;
            }
        }

        public override void EndOfStateCleanup()
        {
            unit.Unsubscribe_UnitClicked(PlayerUnit_UnitClicked);
        }
    }
}
