using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoving", menuName = "Rule/PlayerMoving")]
public class PlayerMovingRuleSO : RuleSO
{
    public Vector2 moveVector = new Vector2(0,1);

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;

        if (CharacterController.instance.inputVec == moveVector)
            return true;

        return false;
    }
}