using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackInstructions", menuName = "Scriptable Objects/AttackInstructions")]
public class AttackInstructions : ScriptableObject
{
    [Header("Attack")]
    public AttackSO attack;  // the actual attack SO

    [Header("Banned Attacks")]
    public List<AttackSO> banned = new List<AttackSO>(); // attacks that can't follow this one (optional)

    [Header("Required Attacks")]
    public List<AttackSO> required = new List<AttackSO>(); // list of attacks that will be chosen from for next move (optional)
}
