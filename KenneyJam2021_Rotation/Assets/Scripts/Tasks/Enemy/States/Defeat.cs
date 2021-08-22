using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Enemy;
using System;
using StateMachine;

public class Defeat : EnemyBaseState
{
    public Defeat()
    {
        SoundManagerScript.PlaySound("EnemyDeath");
    }

    public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
    {
        EnemyTask task = subject as EnemyTask;

        task.Hitbox.enabled = false;
        task.anim.Play("Defeat", 0);
        yield return null;

        float timer = 0f;
        AnimatorStateInfo info = task.anim.GetCurrentAnimatorStateInfo(0);

        while (timer < info.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GameObject.Destroy(task.gameObject);
    }
}
