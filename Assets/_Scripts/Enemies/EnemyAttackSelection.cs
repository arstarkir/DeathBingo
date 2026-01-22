using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSelection : Singleton<EnemyAttackSelection>
{
    [SerializeField] GameObject attackHolder; // object in scene that has the enemies/attacks
    public bool isInAttack = false; // true if any attack is out
    [SerializeField] private bool isWaveRunning = false; // true if wave in progress

    public List<WaveSO> waves = new List<WaveSO>(); // list of waves
    [SerializeField] int curWaveId; // id of current wave
    AttackSO curAttack; // current attack

    // create list of random attacks and wait to start attacking
    void Start()
    {
        isInAttack = false;
        curWaveId = 0;
        StartCoroutine(StartDelay(waves[0].downtime));
    }

    // delay before a wave
    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int boardSize = Mathf.Clamp(curWaveId + 1, 1, 5);
        BingoController.instance.SetBoardSize(boardSize, waves[curWaveId].ruleGroups);
        StartCoroutine(RunWave(waves[curWaveId]));
    }

    // wave running
    private IEnumerator RunWave(WaveSO wave)
    {
        isWaveRunning = true;
        if (wave.introSequence.attacks.Count > 0) // intro attacks
        {
            yield return StartCoroutine(RunSequence(wave.introSequence));
        }
        while (isWaveRunning) // random attacks
        {
            if (wave.attackSequences.Count == 0) break;
            int index = SeedManager.instance.rng.Next(0, wave.attackSequences.Count);
            AttackSequenceSO randomSequence = wave.attackSequences[index];
            yield return StartCoroutine(RunSequence(randomSequence));
        }
        curWaveId++;
        if (curWaveId < waves.Count) // next wave
        {
            StartCoroutine(StartDelay(waves[curWaveId].downtime));
        }
        else
        {
            Debug.Log("Game Won!");
            EndScreenUI.instance.WinScreen();
        }
    }

    // attack sequence running
    private IEnumerator RunSequence(AttackSequenceSO sequence)
    {
        foreach (var block in sequence.attacks)
        {
            if (!isWaveRunning)
            {
                break;
            }
            if (block.waitTime < 0) // wait time -1
            {
                if (block.attack.attackType == AttackSO.AttackType.Primary) // primary will wait until the previous is done if wait time is -1
                {
                    if (isInAttack)
                        yield return new WaitUntil(() => !isInAttack);
                    isInAttack = true;
                }
            }
            else // manually set wait time
            {
                yield return new WaitForSeconds(block.waitTime); // if wait time was specified wait that amount no matter what
            }
            if (!isWaveRunning)
            {
                break;
            }
            curAttack = Instantiate(block.attack);
            curAttack.StartAttack(attackHolder);    
        }
    }

    // end wave function
    public void EndWave()
    {
        isWaveRunning = false;
    }
}
