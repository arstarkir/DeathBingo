using UnityEngine;

public class DamageRuleSO : ScriptableObject
{
    public virtual bool CheckRule(DamageSource source)
    {
        return false;
    }
}
