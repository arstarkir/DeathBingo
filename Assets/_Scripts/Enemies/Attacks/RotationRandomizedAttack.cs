using System;
using UnityEngine;

public class RotationRandomizedAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }

    void Start()
    {
        if (attackSO.onlyRightAngles)
            attackSO.temp.transform.rotation = Quaternion.Euler(0, Convert.ToInt32(RngRange(0, 4)) * 90, 0);
        else
            attackSO.temp.transform.rotation = Quaternion.Euler(0, RngRange(0, 360), 0);
    }

    float RngRange(float min, float max)
    {
        return min + (float)(SeedManager.instance.rng.NextDouble() * (max - min));
    }
}