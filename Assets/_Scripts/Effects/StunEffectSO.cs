using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "Effects/Stun")]
public class StunEffectSO : EffectSO
{
    public override void TriggerEffect(float deltaTime)
    {

    }

    public override void OnEffectStart()
    {
        CharacterController.instance.isInteractable = false;
        base.OnEffectStart();
    }

    public override void OnEffectEnd()
    {
        CharacterController.instance.isInteractable = true;
        base.OnEffectEnd();
    }
}
