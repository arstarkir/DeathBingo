using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveSO : ScriptableObject
{
    [Header("Attack Sequences")]
    public AttackSequenceSO introSequence; // first sequence that always plays
    public List<AttackSequenceSO> attackSequences = new List<AttackSequenceSO>(); // list of sequences randomly pulled from after first

    [Header("Rules")]
    public List<RuleGroupSO> ruleGroups = new List<RuleGroupSO>(); // list of rule groups to pull from

    [Header("Downtime (If first wave, this is starting time!")]
    public float downtime = 1f; // how many seconds to wait before starting wave

    [Header("How much HP should be given to the player at the start of this wave")]
    public int hp = 5;
}
