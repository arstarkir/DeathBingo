using UnityEngine;

[CreateAssetMenu(fileName = "BleedEffectSO", menuName = "Scriptable Objects/BleedEffectSO")]
public class BleedEffectSO : EffectSO
{
    public override void OnEffectStart()
    {
        CharacterController.instance.bleeding = true;
        base.OnEffectStart();
    }

    public override void OnEffectEnd()
    {
        CharacterController.instance.bleeding = false;
        base.OnEffectEnd();
    }
}
