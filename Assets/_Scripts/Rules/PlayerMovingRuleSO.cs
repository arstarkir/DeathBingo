using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoving", menuName = "Rule/PlayerMoving")]
public class PlayerMovingRuleSO : DamageRuleSO
{
    public Vector2 moveVector = new Vector2(0,1);

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (trigger != source.Item1 && trigger != DamageSource.Ignore)
            return false;

        if (attackSO != null && source.Item2 != null && source.Item2.attackSO.name == attackSO.name)
            return false;

        if (CharacterController.instance.inputVec == moveVector)
            return true;

        return false;
    }
}