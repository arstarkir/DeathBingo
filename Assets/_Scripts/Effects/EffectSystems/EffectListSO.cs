using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectList", menuName = "Effects/EffectList")]
public class EffectListSO : ScriptableObject
{
    public List<EffectSO> effects = new List<EffectSO>();

    public int GetEffectId(EffectSO effect)
    {
        int id = effects.FindIndex(a => a.effectName == effect.effectName);
        return id;
    }

    public int GetEffectIdByName(string effectName)
    {
        int id = effects.FindIndex(a => a.effectName == effectName);
        return id;
    }
}