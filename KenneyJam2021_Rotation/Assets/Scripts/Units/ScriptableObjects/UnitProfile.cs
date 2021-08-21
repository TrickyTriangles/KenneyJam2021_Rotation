using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Unit Profile")]
public class UnitProfile : ScriptableObject
{
    [Range(1, 3)] public int MiningStrength = 1;
    [Range(1, 3)] public int CombatStrength = 1;
    [Range(1, 3)] public int BuildingStrength = 1;
}
