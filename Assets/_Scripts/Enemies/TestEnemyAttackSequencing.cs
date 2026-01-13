using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAttackSequencing : Singleton<TestEnemyAttackSequencing>
{
    public float startDelay = 5;
    [SerializeField] GameObject attackHolder;
    public bool isInAttack = false;

    public List<AttackSO> attacks = new List<AttackSO>();
    AttackSO curAttack;
    [SerializeField] int curAttackId;

    void Start()
    {
        isInAttack = true;
        StartCoroutine(StartDelay(startDelay));
    }

    IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isInAttack = false;
    }

    private void Update()
    {
        if (isInAttack)
            return;
        if(curAttackId >= attacks.Count)
        {
            this.enabled = false;
            return;
        }
        curAttack = Instantiate(attacks[curAttackId]);
        curAttack.StartAttack(attackHolder);
        curAttackId++;
        isInAttack = true;
    }
}
