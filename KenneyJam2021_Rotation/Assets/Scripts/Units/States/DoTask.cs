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
            switch (task.GetTaskProfile().type)
            {
                case Task.TaskType.MINING:
                    Debug.Log("Doing mining task.");
                    ICompleteable comp = task;
                    comp.DoTask();
                    break;
                case Task.TaskType.ATTACKING:
                    Debug.Log("Doing attacking task.");
                    PlayerProjectile projectile = GameObject.Instantiate(unit.Projectile, Vector3.zero, unit.transform.rotation).GetComponent<PlayerProjectile>();
                    if (projectile != null)
                    {
                        projectile.Initialize(task);
                    }
                    break;
                case Task.TaskType.RESTING:
                    Debug.Log("Doing resting task.");
                    break;
                default:
                    break;
            }

            while (timer < task.GetTaskTime())
            {
                if (task_ended)
                {
                    next_state_callback?.Invoke(new Move(unit.LastDirection));
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
