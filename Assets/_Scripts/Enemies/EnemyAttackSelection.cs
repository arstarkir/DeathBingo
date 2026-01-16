using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSelection : Singleton<EnemyAttackSelection>
{
    public float startDelay = 5; // amount of time to wait before starting spawning
    public float attackDelay = 3; // time between attacks (arbitrary for now)
    [SerializeField] GameObject attackHolder; // object in scene that has the enemies/attacks
    public bool isInAttack = false; // true if any attack is out

    public List<AttackSO> attacks = new List<AttackSO>(); // list of attacks that can be used
    public List<AttackInstructions> instructions = new List<AttackInstructions>(); // list of instructions applied when choosing attacks
    public int sequenceLength = 5; // how many attacks to spawn

    [SerializeField] private List<AttackSO> randomizedAttacks; // list of randomized attacks
    AttackSO curAttack; // current attack
    [SerializeField] int curAttackId; // id of current attack
    [SerializeField] bool doFirst = false;

    // create list of random attacks and wait to start attacking
    void Start()
    {
        if(doFirst)
        {
            StartCoroutine(StartDelay(startDelay));
            return;
        }
        randomizedAttacks = SeedManager.instance.RandomizeAttacks(attacks, sequenceLength, instructions);

        isInAttack = false;
        StartCoroutine(StartDelay(startDelay));
    }

    // waiting for start time to finish
    IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Spawning());
    }

    // spawning an attack
    IEnumerator Spawning()
    {
        if(doFirst)
        {
            curAttack = Instantiate(attacks[curAttackId]);
            curAttack.StartAttack(attackHolder);
            yield return new();
        }

        while (curAttackId < randomizedAttacks.Count)
        {
            if (randomizedAttacks[curAttackId].attackType == AttackSO.AttackType.Primary)
            {
                if (isInAttack)
                {
                    yield return new WaitUntil(() => !isInAttack);
                }
                isInAttack = true;
            }
            else if (curAttackId > 0)
            {
                yield return new WaitForSeconds(attackDelay);
            }
            curAttack = Instantiate(randomizedAttacks[curAttackId]);
            curAttack.StartAttack(attackHolder);
            curAttackId++;
        }
        this.enabled = false;
    }
}
