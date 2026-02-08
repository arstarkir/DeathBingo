using UnityEngine;

[CreateAssetMenu(fileName = "GroundedRuleSO", menuName = "Scriptable Objects/GroundedRuleSO")]
public class GroundedRuleSO : RuleSO
{
    public bool groundedRequirement = false; // if false, then it means you must be airborne

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;

        bool isGrounded = CharacterController.instance.grounded;
        return groundedRequirement ? isGrounded : !isGrounded;
    }
}
