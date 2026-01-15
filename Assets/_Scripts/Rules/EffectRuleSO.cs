using UnityEngine;

[CreateAssetMenu(fileName = "EffectRule", menuName = "Rule/EffectRule")]
public class EffectRuleSO : DamageRuleSO
{
    public EffectSO effect;

    [Header("if you want to check if there is >= amount of the \"effect\" set effectAmount to more then 0")]
    public int effectAmount = -1;

    public override bool CheckRule(DamageSource source)
    {
        if (trigger != source && trigger != DamageSource.Ignore)
            return false;
        Entity entity = Health.instance.GetComponent<Entity>();
        if (effectAmount < 0 && EffectsManager.instance.IsActiveEffectOnEntity(effect,entity))
            return true;

        if(effectAmount >= 0 && effectAmount < EffectsManager.instance.GetActiveEffectOnEntityAmount(effect, entity))
            return true;
        
        return false;
    }
}