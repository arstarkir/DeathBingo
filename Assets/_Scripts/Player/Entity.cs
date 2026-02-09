using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Entity : MonoBehaviour
{
    public GameObject effectHolderUI;
    public GameObject effectUI;
    List<GameObject> curEffects = new List<GameObject>();

    public void AddEffectUI(EffectSO effect)
    {
        curEffects.Add(Instantiate(effectUI, effectHolderUI.transform));
        curEffects[curEffects.Count - 1].GetComponent<Image>().sprite = effect.sprite;
    }

    public void RemoveEffectUI(EffectSO effect)
    {
        foreach (GameObject p in curEffects)
        {
            if(p.GetComponent<Image>().sprite == effect.sprite)
            {
                curEffects.Remove(p);
                Destroy(p);
                break;
            }
        }
    }
}
