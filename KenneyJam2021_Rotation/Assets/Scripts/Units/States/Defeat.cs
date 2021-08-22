using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Player_Unit
{
    public class Defeat : PlayerUnitBaseState
    {
        public Defeat()
        {
            SoundManagerScript.PlaySound("FriendlyDeath");
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            PlayerUnit unit = subject as PlayerUnit;
            unit.anim.Play("Defeat", 0);
            yield return null;

            AnimatorStateInfo info = unit.anim.GetCurrentAnimatorStateInfo(0);
            float timer = 0f;

            while (timer < info.length)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            GameObject.Destroy(unit.gameObject);
        }
    }
}
