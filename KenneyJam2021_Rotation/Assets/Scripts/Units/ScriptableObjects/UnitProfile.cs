using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Unit Profile")]
public class UnitProfile : ScriptableObject
{
    public float velocity = 10f; // Degrees per second in rotation speed around the planet
    public int base_vitality = 20; // This is the number of times a unit can perform a task before dying
    [Min(1)] public int MiningStrength = 1; // This value determines how many diamonds the unit gets every time they mine (TaskProfile.diamond_reward_per_tick + MiningStrength)
    [Min(1)] public int CombatStrength = 1; // This determines how much damage an enemy receives from the unit's attack

}
