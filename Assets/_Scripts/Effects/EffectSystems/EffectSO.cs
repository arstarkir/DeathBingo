using System;
using UnityEngine;

public enum EffectUse
{
    Standard,
    Permanent,
    Other
}

[CreateAssetMenu(fileName = "GenericEffect", menuName = "Effects/GenericEffect")]
public class EffectSO : ScriptableObject
{
    public string effectName;
    public string effectDescription;
    public Sprite sprite;
    public EffectUse effectUse = EffectUse.Standard;
    public float effectDurationTime = Mathf.Infinity;
    public bool isTimeStacked;
    [HideInInspector] public Entity thisEntity;
    public GameObject effectVFX;
    [HideInInspector] public GameObject curVFX;

    public virtual void TriggerEffect(float deltaTime)
    {

    }

    public virtual void OnEffectStart()
    {
        if (effectVFX != null)
            curVFX = Instantiate(effectVFX, CharacterController.instance.transform);
        // Update UI and Sounds HERE
    }

    public virtual void OnEffectEnd()
    {
        if(curVFX != null)
            Destroy(curVFX);
        // Update UI and Sounds HERE
    }
}
