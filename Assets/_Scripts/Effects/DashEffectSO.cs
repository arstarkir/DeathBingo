using UnityEngine;

//[CreateAssetMenu(fileName = "Dash", menuName = "Effects/Dash")]
public class DashEffectSO : EffectSO
{
    public float strength = 100f;

    public override void TriggerEffect(float deltaTime)
    {

    }

    public override void OnEffectStart()
    {
        base.OnEffectStart();

        if (thisEntity == null)
            return;

        Vector3 dir = (thisEntity.transform.forward).normalized;

        Rigidbody rb = thisEntity.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(dir * strength, ForceMode.Impulse);
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
    }
}
