using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoving", menuName = "Rule/PlayerMoving")]
public class PlayerMovingRuleSO : RuleSO
{
    public Vector3 moveVector = new Vector3(0,1,0); // player movement (based on input except for Y which is literal velocity)
    [Header("If an axis is not enabled, the respective value in the moveVector will be ignored.")]
    public Axis axis = Axis.X | Axis.Z; // axis to consider
    public AxisLogic logic = AxisLogic.GreaterThanOrEqualTo; // if movement must be >=/<=/== to given vector
    [Header("Tolerance is considered when logic is set to 'Equal' or 'NotEqual'")]
    public float tolerance = 0f; // forgiveness for equal and !equal

    // checks is movement fits required vector in certain axis
    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;
        Vector3 input = new Vector3(CharacterController.instance.inputVec.x, CharacterController.instance.rb.linearVelocity.y, CharacterController.instance.inputVec.y);

        if (!CheckAxis(input.x, moveVector.x, Axis.X))
        {
            return false;
        }
        if (!CheckAxis(input.y, moveVector.y, Axis.Y))
        {
            return false;
        }
        if (!CheckAxis(input.z, moveVector.z, Axis.Z))
        {
            return false;
        }
        return true;
    }

    // checks if an axis is relevant, and if it is, if it meets the requirements
    bool CheckAxis(float value, float target, Axis axisCheck)
    {
        if (!axis.HasFlag(axisCheck)) return true;
        switch (logic)
        {
            case AxisLogic.Equal:
                return Mathf.Abs(value - target) <= tolerance;
            case AxisLogic.NotEqual:
                return Mathf.Abs(value - target) > tolerance;
            case AxisLogic.LessThanOrEqualTo:
                return value <= target;
            case AxisLogic.GreaterThanOrEqualTo:
                return value >= target;
        }
        return false;
    }
}

[System.Flags]
// axis that are considered
public enum Axis
{
    None = 0,
    X = 1 << 0,
    Y = 1 << 1,
    Z = 1 << 2
}

// if movement should be >=/<=/== to given vector
public enum AxisLogic
{
    Equal,
    NotEqual,
    LessThanOrEqualTo,
    GreaterThanOrEqualTo
}