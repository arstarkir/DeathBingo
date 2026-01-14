using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomAttack", menuName = "SO/RandomAttack")]
public class RandomAttackSO : AttackSO
{
    public GameObject toSpawn;
    public float attackAmount = 3;
    public float delay = 0.2f;

    public override void StartAttack(GameObject attackHolder)
    {
        temp = Instantiate(attackPref, attackHolder.transform);
        temp.AddComponent<RandomAttack>();
        temp.GetComponent<IAttackHandler>().attackSO = this;
    }

    public override void EndAttack()
    {
        base.EndAttack();
    }
}

public class RandomAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }
    RandomAttackSO ta;

    List<GameObject> curRandomAttacks = new List<GameObject>();

    void Start()
    {
        ta = (RandomAttackSO)attackSO;
        StartCoroutine(EndAfterTime(ta.delay));
    }

    float RngRange(float min, float max)
    {
        return min + (float)(SeedManager.instance.rng.NextDouble() * (max - min));
    }

    public IEnumerator EndAfterTime(float s)
    {
        Bounds b = GetComponent<Collider>().bounds;

        for (int i = 0; i < ta.attackAmount; i++)
        {
            yield return new WaitForSeconds(s);
            curRandomAttacks.Add(Instantiate(ta.toSpawn, transform));
            curRandomAttacks.Last().transform.position = new Vector3(RngRange(b.min.x, b.max.x), 0.1f, RngRange(b.min.z, b.max.z));
        }
    }
}