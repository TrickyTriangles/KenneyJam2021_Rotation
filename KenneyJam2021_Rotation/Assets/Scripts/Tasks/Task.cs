using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task : MonoBehaviour, ICompleteable
{
    public enum TaskType
    {
        MINING,
        ATTACKING,
        RESTING
    }

    public class TaskCompletedEventArgs : EventArgs
    {
        public TaskType type;

        public TaskCompletedEventArgs(TaskType _type)
        {
            type = _type;
        }
    }

    protected int vitality = 1;

    protected EventHandler TaskCompleted;
    public void Subscribe_TaskCompleted(EventHandler sub) { TaskCompleted += sub; }
    public void Unsubscribe_TaskCompleted(EventHandler sub) { TaskCompleted -= sub; }

    void ICompleteable.DoTask(UnitProfile unit)
    {
    }

    public virtual float GetTaskTime()
    {
        return 1f;
    }

    public virtual TaskProfile GetTaskProfile()
    {
        return ScriptableObject.CreateInstance<TaskProfile>();
    }
}