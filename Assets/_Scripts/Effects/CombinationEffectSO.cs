using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CombinationEffect", menuName = "Effects/CombinationEffect")]
public class CombinationEffectSO : EffectSO
{
    public List<EffectSO> effects = new List<EffectSO>();

    public override void TriggerEffect(float deltaTime)
    {
        
    }

    public override void OnEffectStart()
    {
        base.OnEffectStart();
        if (thisEntity.TryGetComponent<Entity>(out Entity entity))
        {
            foreach(EffectSO effect in effects)
            {
                EffectSO temp = Instantiate(effect);
                temp.OnEffectStart();
            }
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
        if (thisEntity.TryGetComponent<Entity>(out Entity entity))
        {
            foreach (EffectSO effect in effects)
            {
                EffectSO temp = Instantiate(effect);
                temp.thisEntity = this.thisEntity;
                temp.OnEffectEnd();
            }
        }
    }
}
