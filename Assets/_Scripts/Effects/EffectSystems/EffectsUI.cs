using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EffectsUI : MonoBehaviour
{
    public GameObject effectVisualizerSpace;
    public GameObject effectItem;
    List<GameObject> curEffects = new List<GameObject>();

    public virtual void AddEffect(EffectSO effect)
    {
        SpawnEffect(effect, effectVisualizerSpace);
    }

    public void SpawnEffect(EffectSO effect, GameObject parant)
    {
        GameObject temp = Instantiate(effectItem, parant.transform);
        temp.GetComponentInChildren<TMP_Text>().text = effect.dataName;
        curEffects.Add(temp);
    }

    public void RemoveEffect(EffectSO effect)
    {
        GameObject temp = curEffects.FindLast(e => e.GetComponentInChildren<TMP_Text>().text == effect.dataName);   
        curEffects.Remove(temp);
        Destroy(temp);
    }
}
