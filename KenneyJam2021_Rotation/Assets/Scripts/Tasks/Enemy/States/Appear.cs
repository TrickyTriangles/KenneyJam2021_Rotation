using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class Appear : EnemyBaseState
    {
        float start_rotation;

        public Appear(float start_value)
        {
            start_rotation = start_value;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            EnemyTask task = subject as EnemyTask;

            if (task.anim != null)
            {
                task.anim.Play("Appear", 0);
                yield return null;
            }

            AnimatorStateInfo info = task.anim.GetCurrentAnimatorStateInfo(0);

            float timer = 0f;
            while (timer < info.length)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            float z = task.transform.rotation.eulerAngles.z;

            if (start_rotation > 180f)
            {
                next_state_callback(new Move(Move.Direction.RIGHT));
            }
            else
            {
                next_state_callback(new Move(Move.Direction.LEFT));
            }
        }
    }
}
