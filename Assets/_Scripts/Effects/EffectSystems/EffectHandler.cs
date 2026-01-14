using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    public EffectSO effectSO;
    public int effectId = -1;
    EffectsUI effectsUI;

    private void Start()
    {
        if(TryGetComponent<EffectsUI>(out effectsUI))
            effectsUI.AddEffect(effectSO);
    }

    private void OnDestroy()
    {
        if(effectsUI != null)
            effectsUI.RemoveEffect(effectSO);
    }
}
