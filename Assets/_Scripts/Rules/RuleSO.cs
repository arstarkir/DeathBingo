using UnityEngine;

[CreateAssetMenu(fileName = "DamageRule", menuName = "Rule/DamageRule")]
public class RuleSO : Data
{
    [Header("Leave as Ignore if it's not important")]
    public DamageSource trigger;
    [Header("Leave it empty if attack is not important")]
    public AttackSO attackSO;

    public bool CoreRuleCheck((DamageSource, IAttackHandler) source)
    {
        dataType = DataType.Rule;

        if (trigger != source.Item1 && trigger != DamageSource.Ignore)
            return false;

        if (attackSO != null && (source.Item2 == null || source.Item2.attackSO.attackPref.name != attackSO.attackPref.name))
            return false;

        return true;
    }

    public virtual bool CheckRule((DamageSource,IAttackHandler) source)
    {
        return CoreRuleCheck(source);
    }
}
