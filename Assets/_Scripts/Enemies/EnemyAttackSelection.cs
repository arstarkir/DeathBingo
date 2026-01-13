using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSelection : Singleton<EnemyAttackSelection>
{
    public float startDelay = 5; // amount of time to wait before starting spawning
    [SerializeField] GameObject attackHolder; // object in scene that has the enemies/attacks
    public bool isInAttack = false; // true if in the middle of an attack

    public List<AttackSO> attacks = new List<AttackSO>(); // list of attacks that can be used
    public List<AttackInstructions> instructions = new List<AttackInstructions>(); // list of instructions applied when choosing attacks
    public int sequenceLength = 5; // how many attacks to spawn

    [SerializeField] private List<AttackSO> randomizedAttacks; // list of randomized attacks
    AttackSO curAttack; // current attack
    [SerializeField] int curAttackId; // id of current attack

    // create list of random attacks and wait to start attacking
    void Start()
    {
        SeedManager.instance.GenerateSeed(); // This is here for now, but assuming we want to use it elsewhere, it should probably be moved eventually so it doesn't end up getting regenerated!
        randomizedAttacks = SeedManager.instance.RandomizeAttacks(attacks, sequenceLength, instructions);

        isInAttack = true;
        StartCoroutine(StartDelay(startDelay));
    }

    // waiting for start time to finish
    IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isInAttack = false;
    }

    // spawn an attack if not attacking
    private void Update()
    {
        if (isInAttack)
            return;
        if (curAttackId >= randomizedAttacks.Count)
        {
            this.enabled = false;
            return;
        }
        curAttack = Instantiate(randomizedAttacks[curAttackId]);
        curAttack.StartAttack(attackHolder);
        curAttackId++;
        isInAttack = true;
    }
}
