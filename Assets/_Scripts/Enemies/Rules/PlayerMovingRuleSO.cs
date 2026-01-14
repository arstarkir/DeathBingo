using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoving", menuName = "Rule/PlayerMoving")]
public class PlayerMovingRuleSO : DamageRuleSO
{
    public Vector2 moveVector = new Vector2(0,1);

    public override bool CheckRule(DamageSource source)
    {
        if (trigger != source && trigger != DamageSource.Ignore)
            return false;

        if (CharacterController.instance.inputVec == moveVector)
            return true;

        return false;
    }
}