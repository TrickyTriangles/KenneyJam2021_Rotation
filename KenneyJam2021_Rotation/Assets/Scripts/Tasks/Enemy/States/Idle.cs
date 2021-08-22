using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Enemy;
using System;
using StateMachine;

public class Idle : EnemyBaseState
{
    private float shot_delay = 3f;

    public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
    {
        EnemyTask task = subject as EnemyTask;
        task.anim.Play("Idle", 0);

        while (true)
        {
            yield return new WaitForSeconds(shot_delay);
            GameObject.Instantiate(task.Projectile, Vector3.zero, task.transform.rotation);
        }
    }
}
