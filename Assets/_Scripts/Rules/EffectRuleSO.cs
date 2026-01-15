using UnityEngine;

[CreateAssetMenu(fileName = "EffectRule", menuName = "Rule/EffectRule")]
public class EffectRuleSO : DamageRuleSO
{
    public EffectSO effect;

    [Header("if you want to check if there is >= amount of the \"effect\" set effectAmount to more then 0")]
    public int effectAmount = -1;

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (trigger != source.Item1 && trigger != DamageSource.Ignore)
            return false;

        if (attackSO != null && source.Item2 != null && source.Item2.attackSO.name == attackSO.name)
            return false;

        Entity entity = Health.instance.GetComponent<Entity>();
        if (effectAmount < 0 && EffectsManager.instance.IsActiveEffectOnEntity(effect,entity))
            return true;

        if(effectAmount >= 0 && effectAmount < EffectsManager.instance.GetActiveEffectOnEntityAmount(effect, entity))
            return true;
        
        return false;
    }
}