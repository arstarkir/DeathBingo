using UnityEngine;

[CreateAssetMenu(fileName = "Influence", menuName = "Effects/Influence")]
public class InfluenceEffectSO : EffectSO
{
    public float strength = 3;
    public Vector3 dir = Vector3.zero;

    [Header("From obj center will rewright the dir")]
    public bool fromObject = false;
    [Header("Gradual will make influence start at zero and builds to given strength")]
    public bool gradual = false;
    public float gradualTime = 1f;
    float gradualProgress = 0f;
    [HideInInspector] public GameObject curObj;
    CharacterController cc;

    public override void TriggerEffect(float deltaTime)
    {
        float appliedStrength = strength;
        if (gradual)
        {
            gradualProgress += deltaTime;
            float t = Mathf.Clamp(gradualProgress / gradualTime, 0, 1);
            appliedStrength *= t;
        }
        if (fromObject)
            cc.influenceVelocity += (curObj.transform.position - cc.transform.position).normalized * appliedStrength;
        if (!fromObject)
            cc.influenceVelocity += dir.normalized * appliedStrength;
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
