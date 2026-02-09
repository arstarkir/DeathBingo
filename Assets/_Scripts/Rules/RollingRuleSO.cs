using UnityEngine;

[CreateAssetMenu(fileName = "RollingRuleSO", menuName = "Scriptable Objects/RollingRuleSO")]
public class RollingRuleSO : RuleSO
{
    public bool rollingRequirement = true; // if true, you must be rolling

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;

        bool isRolling = CharacterController.instance.rolling;
        return rollingRequirement ? isRolling : !isRolling;
    }
}
