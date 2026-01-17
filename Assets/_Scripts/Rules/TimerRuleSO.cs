using UnityEngine;

[CreateAssetMenu(fileName = "TimerRule", menuName = "Rule/TimerRule")]
public class TimerRuleSO : RuleSO
{
    public string mustContain = "1";

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;

        if (TimerController.instance.GetTime().Contains(mustContain))
            return true;

        return false;
    }
}