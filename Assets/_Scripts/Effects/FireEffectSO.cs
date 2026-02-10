using UnityEngine;

[CreateAssetMenu(fileName = "Fire", menuName = "Effects/Fire")]
public class FireEffectSO : EffectSO
{
    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
        if (!CharacterController.instance.rolling) Health.instance.ChangeHealth(-1, DamageSource.Fire, null);
        EffectsManager.instance.RemoveAllEffectOfKindFromEntity(this, thisEntity);
    }
}
