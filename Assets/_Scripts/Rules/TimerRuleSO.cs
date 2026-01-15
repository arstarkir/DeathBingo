using UnityEngine;

[CreateAssetMenu(fileName = "TimerRule", menuName = "Rule/TimerRule")]
public class TimerRuleSO : DamageRuleSO
{
    public string mustContain = "1";

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (trigger != source.Item1 && trigger != DamageSource.Ignore)
            return false;

        if (attackSO != null && source.Item2 != null && source.Item2.attackSO.name == attackSO.name)
            return false;

        if (TimerController.instance.GetTime().Contains(mustContain))
            return true;

        return false;
    }
}