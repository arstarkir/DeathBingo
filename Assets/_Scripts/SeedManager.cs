using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SeedManager : Singleton<SeedManager>
{
    public int currentSeed { get; private set; }   // number for current seed
    public System.Random rng { get; private set; } // rng value determined by seed (aka what is actually used)

    [Header("if At Start Set Seed = 0 will be generated a new seed on start")]
    public int atStartSetSeed = 0;
    [SerializeField] TMP_Text seedText;

    void Awake()
    {
        base.Awake();
        GenerateSeed();
    }

    // get a new random seed (from 0 to max int)
    public void GenerateSeed()
    {
        currentSeed = UnityEngine.Random.Range(0, int.MaxValue);
        if (atStartSetSeed != 0)
            currentSeed = atStartSetSeed;
        rng = new System.Random(currentSeed);
        seedText.text = currentSeed.ToString();
    }

    // setter for current seed
    public void SetSeed(int seed)
    {
        currentSeed = seed;
        rng = new System.Random(currentSeed);
        seedText.text = currentSeed.ToString();
    }

    // send in a list of attacks, the num of attacks to pick, and a list of any attack instructions.  Get back a list of randomized attacks
    public List<AttackSO> RandomizeAttacks(List<AttackSO> attacks, int length, List<AttackInstructions> instructions)
    {
        List<AttackSO> newList = new List<AttackSO>();
        AttackInstructions previousInstruction = null;

        for (int i = 0; i < length; i++)
        {
            AttackInstructions currentInstruction;

            if ((previousInstruction != null) && (previousInstruction.required.Count) > 0) // if there are required attacks from the last one
            {
                int index = rng.Next(previousInstruction.required.Count);
                AttackSO nextAttack = previousInstruction.required[index];

                currentInstruction = instructions.FirstOrDefault(r => r.attack == nextAttack); // this is probably a lazy/ineffecient way of doing this, will likely be changed
                if (currentInstruction == null) // if there are no instructions, you just assume any attack can follow and nothing is required
                {
                    currentInstruction = ScriptableObject.CreateInstance<AttackInstructions>();
                    currentInstruction.attack = nextAttack;
                    currentInstruction.banned = new List<AttackSO>();
                    currentInstruction.required = new List<AttackSO>();
                }
            }
            else // there are no required attacks
            {
                AttackSO newAttack;
                if (previousInstruction != null && previousInstruction.banned.Count > 0) // if stuff is banned
                {
                    List<AttackSO> valid = new List<AttackSO>();
                    foreach (var attack in attacks)
                    {
                        if (previousInstruction.banned.Contains(attack))
                        {
                            continue;
                        }
                        valid.Add(attack);
                    }

                    if (valid.Count == 0) // if everything is banned (doubt this would ever be the case) allow anything
                    {
                        valid = new List<AttackSO>(attacks);
                    }

                    int index = rng.Next(valid.Count);
                    newAttack = valid[index];
                }
                else // if nothing is banned
                {
                    int index = rng.Next(attacks.Count);
                    newAttack = attacks[index];
                }

                currentInstruction = instructions.FirstOrDefault(r => r.attack == newAttack); // again, using this lazy method to find the attacks instructions and making a blank one if there is none
                if (currentInstruction == null)
                {
                    currentInstruction = ScriptableObject.CreateInstance<AttackInstructions>();
                    currentInstruction.attack = newAttack;
                    currentInstruction.banned = new List<AttackSO>();
                    currentInstruction.required = new List<AttackSO>();
                }
            }
            newList.Add(currentInstruction.attack);
            previousInstruction = currentInstruction;
        }
        return newList;
    }
}
