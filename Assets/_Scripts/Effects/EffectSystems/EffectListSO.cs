using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectList", menuName = "SOList/EffectList")]
public class EffectListSO : ScriptableObject
{
    public List<EffectSO> effects = new List<EffectSO>();

    public int GetEffectId(EffectSO effect)
    {
        int id = effects.FindIndex(a => a.dataName == effect.dataName);
        return id;
    }

    public int GetEffectIdByName(string effectName)
    {
        int id = effects.FindIndex(a => a.dataName == effectName);
        return id;
    }
}