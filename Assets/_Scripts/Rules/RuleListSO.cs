using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleList", menuName = "SOList/RuleList")]
public class RuleListSO : ScriptableObject
{
    public List<RuleSO> rules = new List<RuleSO>();

    public int GetRuleId(RuleSO rule)
    {
        int id = rules.FindIndex(a => a.dataName == rule.dataName);
        return id;
    }

    public int GetRuleIdByName(string ruleName)
    {
        int id = rules.FindIndex(a => a.dataName == ruleName);
        return id;
    }
}