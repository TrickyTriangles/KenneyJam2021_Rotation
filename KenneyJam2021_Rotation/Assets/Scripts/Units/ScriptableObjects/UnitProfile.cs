using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Unit Profile")]
public class UnitProfile : ScriptableObject
{
    public float velocity = 10f;
    [Min(1)] public int MiningStrength = 1;
    [Min(1)] public int CombatStrength = 1;

}
