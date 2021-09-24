using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;
using StateMachine;

public class DoTask : PlayerUnitBaseState
{
    bool task_ended = false;
    PlayerUnit unit;
    Task task;

    public DoTask(Task _task)
    {
        task = _task;
    }

    private void Task_TaskCompleted(object sender, EventArgs args)
    {
        task_ended = true;
    }

    public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
    {
        unit = subject as PlayerUnit;
        task.Subscribe_TaskCompleted(Task_TaskCompleted);

        unit.anim.Play("Mining", 0);

        float timer = 0f;

        while (true)
        {
            // Perform task
            switch (task.GetTaskProfile().type)
            {
                case Task.TaskType.MINING:
                    ICompleteable comp = task;
                    comp.DoTask(unit.Profile);
                    break;
                case Task.TaskType.ATTACKING:
                    PlayerProjectile projectile = GameObject.Instantiate(unit.Projectile, Vector3.zero, unit.transform.rotation).GetComponent<PlayerProjectile>();
                    if (projectile != null)
                    {
                        projectile.Initialize(task, unit.Profile);
                    }
                    break;
                case Task.TaskType.RESTING:
                    break;
                default:
                    break;
            }

            // Damage unit
            unit.LoseVitality(1);
            if (!unit.Alive) { break; }

            yield return null;

            // Wait for next tick
            while (timer < task.GetTaskTime())
            {
                if (task_ended)
                {
                    if (unit.Alive)
                    {
                        next_state_callback?.Invoke(new Move(unit.LastDirection));
                    }
                    else
                    {
                        next_state_callback?.Invoke(new Defeat());
                    }
                }

                if (task.transform.rotation.eulerAngles.z > unit.transform.rotation.eulerAngles.z)
                {
                    unit.SetDirection(PlayerUnit.Direction.LEFT);
                }
                else
                {
                    unit.SetDirection(PlayerUnit.Direction.RIGHT);
                }

                timer += Time.deltaTime;
                yield return null;
            }

            timer -= task.GetTaskTime();
            yield return null;
        }
    }
}
