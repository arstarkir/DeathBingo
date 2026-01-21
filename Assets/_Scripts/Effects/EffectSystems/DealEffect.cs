using System.Linq;
using UnityEngine;

public class DealEffect : MonoBehaviour
{
    public EffectSO effect;
    public bool destroyOnDealDmg = false;
    public bool tryRemoveOnExit = false;
    Entity entity;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !this.enabled)
            return;

        entity = other.transform.root.GetComponentInChildren<Entity>();
        GiveEffect();
    }

    private void OnTriggerStay(Collider other)
    {
        if(!destroyOnDealDmg)
            return;

        if (!other.CompareTag("Player") || !this.enabled)
            return;

        entity = other.transform.root.GetComponentInChildren<Entity>();
        GiveEffect();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !this.enabled)
            return;

        entity = other.transform.root.GetComponentInChildren<Entity>();

        if(tryRemoveOnExit)
        {
            if(EffectsManager.instance.IsActiveEffectOnEntity(effect, entity))
                EffectsManager.instance.TryRemoveFirstEffectOfKindFromEntity(effect, entity);
        }
    }

    public void GiveEffect()
    {
        EffectHandler handler = EffectsManager.instance.AddEffectToEntityForTime(effect, entity, effect.effectDurationTime, gameObject);

        if (handler.effectSO is InfluenceEffectSO)
            ((InfluenceEffectSO)handler.effectSO).curObj = this.gameObject;

        // This HAS to be changed (it's late rn)
        if (effect.dataName.Contains("Stun") && EffectsManager.instance.
            IsActiveEffectOnEntity(EffectsManager.instance.effectsList.effects[0], entity))
            Health.instance.ChangeHealth(-1, DamageSource.Lightning, transform.root.GetComponentsInChildren<IAttackHandler>().First());

        // well if that gets changed, this probably should too... but it's still late!
        if (effect.dataName.Contains("Wet"))
        {
            EffectSO fire = EffectsManager.instance.effectsList.effects.FirstOrDefault(e => e.dataName.Contains("Fire"));
            if (fire != null)
            {
                EffectsManager.instance.RemoveAllEffectOfKindFromEntity(fire, entity);
                foreach (EffectTimer timer in EffectsManager.instance.activeTimers.ToList())
                {
                    if (timer.IsThisEffectTimer(fire, entity) && timer.effectHendler != null)
                    {
                        if (timer.effectHendler.effectSO.curVFX != null)
                            Destroy(timer.effectHendler.effectSO.curVFX);
                        EffectsManager.instance.activeTimers.Remove(timer);
                    }
                }
            }
        }
        if (effect.dataName.Contains("Fire"))
        {
            EffectSO wet = EffectsManager.instance.effectsList.effects.FirstOrDefault(e => e.dataName.Contains("Wet"));
            if (wet != null)
            {
                EffectsManager.instance.RemoveAllEffectOfKindFromEntity(wet, entity);
                foreach (EffectTimer timer in EffectsManager.instance.activeTimers.ToList())
                {
                    if (timer.IsThisEffectTimer(wet, entity) && timer.effectHendler != null)
                    {
                        if (timer.effectHendler.effectSO.curVFX != null)
                            Destroy(timer.effectHendler.effectSO.curVFX);
                        EffectsManager.instance.activeTimers.Remove(timer);
                    }
                }
            }
        }

        if (destroyOnDealDmg)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (tryRemoveOnExit && entity != null)
        {
            if (EffectsManager.instance.IsActiveEffectOnEntity(effect, entity))
                EffectsManager.instance.TryRemoveFirstEffectOfKindFromEntity(effect, entity);
        }
    }
}
