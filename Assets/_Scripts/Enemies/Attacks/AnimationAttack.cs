using UnityEngine;

public class AnimationAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }

    public void EndAttack()
    {
        attackSO.EndAttack();
    }
}