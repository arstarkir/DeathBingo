using System;
using System.Collections.Generic;

[Serializable]
public struct Damage
{
    public DamageSource damageSource;
    public List<DamageRuleSO> damageRules;
}

public enum DamageSource
{
    Ignore,
    Spike,
    Column,
    Fire,
    Other
}