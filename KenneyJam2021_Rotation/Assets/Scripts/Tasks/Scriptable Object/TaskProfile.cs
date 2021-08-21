using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Task Profile")]
public class TaskProfile : ScriptableObject
{
    public Task.TaskType type;
    public int base_vitality;
    public float base_time_per_tick;
}
