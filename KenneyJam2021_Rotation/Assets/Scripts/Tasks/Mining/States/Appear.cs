using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateMachine._MiningTask
{
    public class Appear : MiningTaskBaseState
    {
        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            MiningTask task = subject as MiningTask;

            if (task.anim != null)
            {
                task.anim.Play("Appear", 0);
            }

            yield return null;

            AnimatorStateInfo info = task.anim.GetCurrentAnimatorStateInfo(0);
            float timer = 0f;

            while (timer < info.length)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
