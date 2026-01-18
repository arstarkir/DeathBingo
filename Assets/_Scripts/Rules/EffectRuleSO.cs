using UnityEngine;

[CreateAssetMenu(fileName = "EffectRule", menuName = "Rule/EffectRule")]
public class EffectRuleSO : RuleSO
{
    public EffectSO effect;

    [Header("if you want to check if there is >= amount of the \"effect\" set effectAmount to more then 0")]
    public int effectAmount = -1;
    public bool isAccumulatedTime = false;
    [Header("ignore if isAccumulatedTime set to false")]
    public float overDuration = 25;

    public override bool CheckRule((DamageSource, IAttackHandler) source)
    {
        if (!CoreRuleCheck(source))
            return false;

        Entity entity = Health.instance.GetComponent<Entity>();
        if (effectAmount < 0 && EffectsManager.instance.IsActiveEffectOnEntity(effect,entity))
        {
            if (isAccumulatedTime)
            {
                if (EffectsManager.instance.GetLongestRemainingEffectOnEntity(effect, entity) > overDuration)
                    return true;
            }
            else
                return true;
        }

        if (effectAmount >= 0 && effectAmount < EffectsManager.instance.GetActiveEffectOnEntityAmount(effect, entity))
        {
            if (isAccumulatedTime)
            {
                if (EffectsManager.instance.GetLongestRemainingEffectOnEntity(effect, entity) > overDuration)
                    return true;
            }
            else
                return true;
        }
        
        return false;
    }
}