using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboRule", menuName = "SO/ComboRule")]
public class ComboRuleSO : DamageRuleSO
{
    public List<DamageRuleSO> rules = new List<DamageRuleSO>();

    public override bool CheckRule(DamageSource source)
    {
        if (trigger != source && trigger != DamageSource.Ignore)
            return false;

        return rules.All(rule => rule.CheckRule(source));
    }
}