using System.Linq;
using UnityEngine;

public class DealEffect : MonoBehaviour
{
    public EffectSO effect;
    public bool destroyOnDealDmg = false;
    public bool tryRemoveOnExit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !this.enabled)
            return;
        GiveEffect(other.transform.root.GetComponent<Entity>());
    }

    private void OnTriggerStay(Collider other)
    {
        if(!destroyOnDealDmg)
            return;

        if (!other.CompareTag("Player") || !this.enabled)
            return;

        GiveEffect(other.transform.root.GetComponent<Entity>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !this.enabled)
            return;

        Entity entity = other.GetComponent<Entity>();

        if(tryRemoveOnExit)
        {
            if(EffectsManager.instance.IsActiveEffectOnEntity(effect, entity))
                EffectsManager.instance.RemoveAllEffectOfKindFromEntity(effect, entity);
        }
    }

    public void GiveEffect(Entity entity)
    {
        EffectHandler handler = EffectsManager.instance.AddEffectToEntityForTime(effect, entity, effect.effectDurationTime, gameObject);

        if (handler.effectSO is InfluenceEffectSO)
            ((InfluenceEffectSO)handler.effectSO).curObj = this.gameObject;

        // This HAS to be changed (it's late rn)
        if (effect.effectName.Contains("Stun") && EffectsManager.instance.
            IsActiveEffectOnEntity(EffectsManager.instance.effectsList.effects[0], entity))
            Health.instance.ChangeHealth(-1, DamageSource.Lightning, transform.root.GetComponentsInChildren<IAttackHandler>().First());

        if (destroyOnDealDmg)
            Destroy(gameObject);
    }
}
