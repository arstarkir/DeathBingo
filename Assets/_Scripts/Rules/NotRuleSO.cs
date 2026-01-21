using UnityEngine;

[CreateAssetMenu(fileName = "NotRule", menuName = "Rule/NotRule")]
public class NotRuleSO : RuleSO
{
    [SerializeField] private RuleSO falseRule; // rule to be NOT true

    // override for checkrule that just swaps true and false
    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        return !falseRule.CheckRule(source);
    }
}
