using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateMachine._MiningTask
{
    public class Idle : MiningTaskBaseState
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
