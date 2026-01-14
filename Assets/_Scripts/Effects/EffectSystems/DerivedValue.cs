using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DerivedValue
{
    [SerializeField] private float baseValue;
    private List<float> multipliers = new List<float>();

    public delegate void OnVariableChangeDelegate(float prev, float cur);
    public event OnVariableChangeDelegate OnVariableChange;

    public float BaseValue
    {
        get { return baseValue; }
        set
        {
            baseValue = value;
            CalculateDerivedValue();
        }
    }

    private float curDerivedValue = 0;
    public float derivedValue
    {
        get 
        {
            return curDerivedValue;
        }
        set
        {
            if (OnVariableChange != null)
                OnVariableChange(curDerivedValue,value);
            curDerivedValue = value;
        }
    }

    public DerivedValue(float baseValue)
    {
        this.baseValue = baseValue;
        CalculateDerivedValue();
    }

    public void CalculateDerivedValue()
    {
        derivedValue = baseValue;
        foreach (var multiplier in multipliers)
            derivedValue *= multiplier;
    }

    public void AddMultiplier(float multiplier)
    {
        multipliers.Add(multiplier);
        CalculateDerivedValue();
    }

    public void RemoveMultiplier(float multiplier)
    {
        if (multipliers.Contains(multiplier))
        {
            multipliers.Remove(multiplier);
            CalculateDerivedValue();
        }
    }
}