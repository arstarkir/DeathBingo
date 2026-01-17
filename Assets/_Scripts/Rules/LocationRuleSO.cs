using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "Rule/Location")]
public class LocationRuleSO : RuleSO
{
    [Header("Field spans (-10,1,-10) to (10,inf,10)")]
    public Vector3 MinBounds = new Vector3(-10, 0, -10);
    public Vector3 MaxBounds = new Vector3(10, 10, 10);
    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;

        Vector3 playerPosition = CharacterController.instance.GetComponent<Collider>().bounds.center;
        if (playerPosition.x < MinBounds.x || playerPosition.y < MinBounds.y || playerPosition.z < MinBounds.z)
            return false;
        if (playerPosition.x > MaxBounds.x || playerPosition.y > MaxBounds.y || playerPosition.z > MaxBounds.z)
            return false;

        return true;
    }
}