using UnityEngine;

[CreateAssetMenu(fileName = "Influence", menuName = "Effects/Influence")]
public class InfluenceEffectSO : EffectSO
{
    public float strength = 3;
    public Vector3 dir = Vector3.zero;

    [Header("From obj center will rewright the dir")]
    public bool fromObject = false;
    [HideInInspector] public GameObject curObj;
    CharacterController cc;

    public override void TriggerEffect(float deltaTime)
    {
        if (fromObject)
            cc.influenceVelocity += (curObj.transform.position - cc.transform.position).normalized * strength * deltaTime;
        if (!fromObject)
            cc.influenceVelocity += dir.normalized * strength * deltaTime;
    }

    public override void OnEffectStart()
    {
        cc = CharacterController.instance;
        base.OnEffectStart();
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
    }
}
