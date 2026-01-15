using UnityEngine;

public class DealEffect : MonoBehaviour
{
    public EffectSO effect;
    public bool destroyOnDealDmg = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !this.enabled)
            return;

        Entity entity = other.transform.root.GetComponent<Entity>();
        EffectHandler handler = EffectsManager.instance.AddEffectToEntityForTime(effect, entity, effect.effectDurationTime, gameObject);

        // This HAS to be changed (it's late rn)
        if (effect.effectName.Contains("Stun") && EffectsManager.instance.
            IsActiveEffectOnEntity(EffectsManager.instance.effectsList.effects[0], entity))
            Health.instance.ChangeHealth(-1, DamageSource.Lightning);

        if(destroyOnDealDmg)
            Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!destroyOnDealDmg)
            return;

        if (!other.CompareTag("Player") || !this.enabled)
            return;

        Entity entity = other.transform.root.GetComponent<Entity>();
        EffectHandler handler = EffectsManager.instance.AddEffectToEntityForTime(effect, entity, effect.effectDurationTime, gameObject);

        // This HAS to be changed (it's late rn)
        if (effect.effectName.Contains("Stun") && EffectsManager.instance.
            IsActiveEffectOnEntity(EffectsManager.instance.effectsList.effects[0], entity))
            Health.instance.ChangeHealth(-1, DamageSource.Lightning);

        if (destroyOnDealDmg)
            Destroy(gameObject);
    }
}
