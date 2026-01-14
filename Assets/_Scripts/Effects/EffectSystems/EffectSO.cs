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

    public virtual void TriggerEffect(float deltaTime)
    {

    }

    public virtual void OnEffectStart()
    {
        // Update UI and Sounds HERE
    }

    public virtual void OnEffectEnd()
    {
        // Update UI and Sounds HERE
    }
}
