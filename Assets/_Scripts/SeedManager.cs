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
}
