using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine._MiningTask
{
    public class Defeat : MiningTaskBaseState
    {
        public Defeat()
        {
            SoundManagerScript.PlaySound("RocksBreaking");
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            MiningTask task = subject as MiningTask;

            if (task.anim != null)
            {
                task.anim.Play("Destroy", 0);
                yield return null;

                AnimatorStateInfo info = task.anim.GetCurrentAnimatorStateInfo(0);
                float timer = 0f;

                while (timer < info.length)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                GameObject.Destroy(task.gameObject);
            }
        }
    }
}