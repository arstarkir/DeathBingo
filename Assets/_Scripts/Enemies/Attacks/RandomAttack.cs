using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }
    List<GameObject> curRandomAttacks = new List<GameObject>();

    void Start()
    {
        StartCoroutine(EndAfterTime(attackSO.delay));
    }

    float RngRange(float min, float max)
    {
        return min + (float)(SeedManager.instance.rng.NextDouble() * (max - min));
    }

    public IEnumerator EndAfterTime(float s)
    {
        Bounds b = GetComponent<Collider>().bounds;

        for (int i = 0; i < attackSO.attackAmount; i++)
        {
            yield return new WaitForSeconds(s);
            curRandomAttacks.Add(Instantiate(attackSO.toSpawn, transform));
            curRandomAttacks.Last().transform.position = new Vector3(RngRange(b.min.x, b.max.x), 0.1f, RngRange(b.min.z, b.max.z));
        }
    }
}