using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleGroup", menuName = "Rule/Rule Group")]
public class RuleGroupSO : ScriptableObject
{
    public List<RuleSO> rules = new List<RuleSO>(); // list of rules
    public int maxSelected = 1; // how many rules will be used at a time from this group
}
