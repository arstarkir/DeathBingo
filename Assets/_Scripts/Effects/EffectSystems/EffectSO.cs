using System;
using UnityEngine;

public enum EffectUse
{
    Standard,
    Permanent,
    Other
}

[CreateAssetMenu(fileName = "GenericEffect", menuName = "Effects/GenericEffect")]
public class EffectSO : Data
{
    public Sprite sprite;
    public AudioClip onStartSound;
    public AudioClip onEndSound;
    public EffectUse effectUse = EffectUse.Standard;
    public float effectDurationTime = Mathf.Infinity;
    public bool isTimeStacked;
    public bool isResetStacked;
    [HideInInspector] public Entity thisEntity;
    public GameObject effectVFX;
    [HideInInspector] public GameObject curVFX;

    public virtual void TriggerEffect(float deltaTime)
    {

    }

    public virtual void OnEffectStart()
    {
        dataType = DataType.Effect;
        wasDone = true;
        PlayerProgressTracker.instance.UpdateProgressData(dataName, wasDone, dataType);

        if (effectVFX != null)
            curVFX = Instantiate(effectVFX, CharacterController.instance.transform);

        thisEntity.AddEffectUI(this);

        if(onStartSound != null)
            AudioManager.instance.PlaySFX(onStartSound, true, thisEntity.transform, 2);
    }

    public virtual void OnEffectEnd()
    {
        if(curVFX != null)
            Destroy(curVFX);

        thisEntity.RemoveEffectUI(this);

        if (onEndSound != null)
            AudioManager.instance.PlaySFX(onEndSound, true, thisEntity.transform, 2);
    }
}