using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Task Profile")]
public class TaskProfile : ScriptableObject
{
    public Task.TaskType type; // Type of task. "Resting" goes unused
    public int base_vitality; // Number of times this task will have to be performed in order to finish it
    public float base_time_per_tick; // How long it takes to perform one action of this task
    public int diamond_reward_per_tick; // For mining tasks: this is the minimal amount of diamonds you can receieve from each mining action
    public int shot_power; // For enemy tasks: This is how much an enemy's shot will decrease a unit's vitality
}
