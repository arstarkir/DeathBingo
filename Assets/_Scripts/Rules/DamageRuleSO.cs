using UnityEngine;

[CreateAssetMenu(fileName = "DamageRule", menuName = "Rule/DamageRule")]
public class DamageRuleSO : ScriptableObject
{
    public string ruleName;
    public string hoverDescription;

    [Header("Leave as Ignore if it's not important")]
    public DamageSource trigger;
    [Header("Leave it empty if attack is not important")]
    public AttackSO attackSO;

    public virtual bool CheckRule((DamageSource,IAttackHandler) source)
    {
        if (trigger != source.Item1 && trigger != DamageSource.Ignore)
            return false;

        if(attackSO != null && source.Item2 != null && source.Item2.attackSO.name == attackSO.name)
            return false;

        return true;
    }
}
