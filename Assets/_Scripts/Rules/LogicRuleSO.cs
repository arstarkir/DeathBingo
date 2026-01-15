using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboRule", menuName = "Rule/ComboRule")]
public class LogicRuleSO : DamageRuleSO
{
    public LogicRuleEnum ruleFlag;
    public List<DamageRuleSO> rules = new List<DamageRuleSO>();
    public int andAmount = 5;

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (trigger != source.Item1 && trigger != DamageSource.Ignore)
            return false;

        if (attackSO != null && (source.Item2 == null || source.Item2.attackSO.attackPref.name != attackSO.attackPref.name))
            return false;

        if (ruleFlag == LogicRuleEnum.And)
            return rules.All(rule => rule.CheckRule(source));

        if (ruleFlag == LogicRuleEnum.AndAmount)
            return rules.FindAll(rule => rule.CheckRule(source)).Count >= andAmount;

        if (ruleFlag == LogicRuleEnum.Or)
            return rules.FindAll(rule => rule.CheckRule(source)).Count >= 1;

        return false;
    }
}

public enum LogicRuleEnum
{
    And = 0,
    AndAmount = 1,
    Or = 2,
}