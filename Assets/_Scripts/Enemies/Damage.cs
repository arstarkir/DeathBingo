using System;

[Serializable]
public struct Damage
{
    public DamageSource damageSource;
    public DamageRuleSO damageRule;
}

public enum DamageSource
{
    Ignore,
    Spike,
    Column,
    Fire,
    Other
}