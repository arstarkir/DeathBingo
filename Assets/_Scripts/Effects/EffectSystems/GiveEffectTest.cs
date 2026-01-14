using UnityEngine;

public class GiveEffectTest : MonoBehaviour
{
    public EffectSO effect;
    public bool destroyOnGive = false;

    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            EffectHandler handler = EffectsManager.instance.AddEffectToEntityForTime(effect, 
                other.transform.root.GetComponent<Entity>(), effect.effectDurationTime, gameObject);

            if (destroyOnGive)
                Destroy(gameObject);
        }
    }
}
