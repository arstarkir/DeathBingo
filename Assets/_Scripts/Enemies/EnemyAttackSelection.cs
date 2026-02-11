using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSelection : Singleton<EnemyAttackSelection>
{
    [SerializeField] GameObject attackHolder; // object in scene that has the enemies/attacks
    public int primaryAttackCount = 0; // how many primary attacks are in progress
    public bool isWaveRunning = false; // true if wave in progress

    public List<WaveSO> waves = new List<WaveSO>(); // list of waves
    [SerializeField] int curWaveId; // id of current wave
    AttackSO curAttack; // current attack

    public bool AbruptWaveTransition = true; // if true, attacks will be instantly ended when Bingo is earned
    CharacterController characterController;
    // create list of random attacks and wait to start attacking
    void Start()
    {
        primaryAttackCount = 0;
        curWaveId = 0;
        characterController = CharacterController.instance;
        StartCoroutine(StartDelay(waves[0].downtime));
    }

    // delay before a wave
    private IEnumerator StartDelay(float delay)
    {
        if (AbruptWaveTransition)
        {
            foreach (Transform child in attackHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            yield return new WaitUntil(() => attackHolder.transform.childCount == 0);
        }
        primaryAttackCount = 0;
        Health.instance.SetHealth(waves[curWaveId].hp);
        int boardSize = Mathf.Clamp(curWaveId + 1, 1, 5);
        BingoController.instance.SetBoardSize(boardSize, waves[curWaveId].ruleGroups);
        yield return new WaitForSeconds(delay);

        float tDelay = delay;
        while (tDelay > 0f)
        {
            if (!isWaveRunning) 
                yield break;

            if (characterController.isPaused)
                yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);

            tDelay -= Time.deltaTime;
            yield return null;
        }

        if (characterController.isPaused)
            yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);

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
            if (characterController.isPaused)
                yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);

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
            foreach (Transform child in attackHolder.transform)
            {
                Destroy(child.gameObject);
            }
            primaryAttackCount = 0;
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

            if (characterController.isPaused)
                yield return new WaitUntil(() => (!characterController.isPaused && isWaveRunning) || primaryAttackCount == 0);

            if (characterController.isPaused && primaryAttackCount == 0)
                yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);

            if (block.waitTime >= 0)
            {
                float t = block.waitTime;
                while (t > 0f)
                {
                    if (!isWaveRunning) yield break;

                    if (characterController.isPaused)
                        yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);

                    t -= Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                if (primaryAttackCount > 0)
                    yield return new WaitUntil(() => primaryAttackCount == 0 || !isWaveRunning);

                if (characterController.isPaused)
                    yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);
            }

            if (block.attack.attackType == AttackSO.AttackType.Primary)
            {
                primaryAttackCount++;
            }

            if (!isWaveRunning)
            {
                break;
            }

            if (characterController.isPaused)
                yield return new WaitUntil(() => !characterController.isPaused || !isWaveRunning);

            curAttack = Instantiate(block.attack);
            curAttack.spawnLocation = block.spawnLocation;
            curAttack.StartAttack(attackHolder);    
        }
    }

    // end wave function
    public void EndWave()
    {
        isWaveRunning = false;
    }
}
