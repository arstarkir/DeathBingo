using UnityEngine;

[CreateAssetMenu(fileName = "DamageRule", menuName = "SO/DamageRule")]
public class DamageRuleSO : ScriptableObject
{
    public string ruleName;
    public string hoverDescription;
    public DamageSource trigger;

    public virtual bool CheckRule(DamageSource source)
    {
        if (trigger != source && trigger != DamageSource.Ignore)
            return false;

        return true;
    }
}
