using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BingoController : Singleton<BingoController>
{
    public GameObject slotPref;
    public GameObject slotHolder;

    public List<RuleSO> activeRules = new List<RuleSO>();
    List<RuleSO> finishedRules = new List<RuleSO>();
    List<BingoSlotUI> curSlots = new List<BingoSlotUI>();   
    int[][] validBingos = // list of valid bingo lines for 5x5
    {
        // rows
        new int[] { 0, 1, 2, 3, 4 },
        new int[] { 5, 6, 7, 8, 9 },
        new int[] { 10, 11, 12, 13, 14 },
        new int[] { 15, 16, 17, 18, 19 },
        new int[] { 20, 21, 22, 23, 24 },

        // columns
        new int[] { 0, 5, 10, 15, 20 },
        new int[] { 1, 6, 11, 16, 21 },
        new int[] { 2, 7, 12, 17, 22 },
        new int[] { 3, 8, 13, 18, 23 },
        new int[] { 4, 9, 14, 19, 24 },

        // diagonals
        new int[] { 0, 6, 12, 18, 24 },
        new int[] { 4, 8, 12, 16, 20 }
    };

    List<int> bingoIDList = new List<int>(); // list of ID of Bingos achieved (.Count to see how many Bingos a player has)

    [HideInInspector] public int maxRuleCombo = 0;

    public void Start()
    {
        Shuffle(activeRules, SeedManager.instance.rng);
        foreach (RuleSO rule in activeRules)
        {
            BingoSlotUI temp = Instantiate(slotPref, slotHolder.transform).GetComponent<BingoSlotUI>();
            temp.SetDamageRule(rule);
            curSlots.Add(temp);
        }
    }

    // put bingo squares in random order and cap at 25
    void Shuffle<DamageRuleSO>(List<DamageRuleSO> list, System.Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        if (list.Count > 25)
        {
            list.RemoveRange(25, list.Count - 25);
        }
    }

    public void RuleCheck(List<(DamageSource, IAttackHandler)> damageInfo)
    {
        foreach ((DamageSource, IAttackHandler) info in damageInfo) // this will be a max of 3 so it should be fine
        {
            foreach(RuleSO rule in activeRules)
            {
                if(rule.CheckRule(info))
                {
                    finishedRules.Add(rule);
                    rule.SetWasDone(true);
                    PlayerProgressTracker.instance.UpdateProgressData(rule.dataName, rule.wasDone, DataType.Rule);
                }
            }
        }

        if (finishedRules.Count > 0)
        {
            if(finishedRules.Count > maxRuleCombo)
                maxRuleCombo = finishedRules.Count;
            FinishRules();
        }
    }

    public void FinishRules()
    {
        List<BingoSlotUI> active = curSlots.Where(slot => !slot.finished).ToList();

        foreach (RuleSO rule in finishedRules)
        {
            foreach (BingoSlotUI slot in active.Where(s => s.rule == rule))
            {
                slot.FinishRule();
            }
            activeRules.RemoveAll(r => r == rule);
        }
        finishedRules.Clear();
        CheckForBingo();
    }

    // check all valid bingos to see if a new one has been earned
    public void CheckForBingo()
    {
        HashSet<int> finishedSlots = new HashSet<int>();
        for (int i = 0; i < curSlots.Count; i++)  // list of id of squares earned
        {
            if (curSlots[i].finished)
            {
                finishedSlots.Add(i);
            }
        }

        for (int bingoId = 0; bingoId < validBingos.Length; bingoId++)
        {
            if (bingoIDList.Contains(bingoId)) continue;
            bool isBingo = true;
            foreach (int slot in validBingos[bingoId])
            {
                if (!finishedSlots.Contains(slot))
                {
                    isBingo = false;
                    break;
                }
            }
            if (isBingo)
            {
                GetBingo(bingoId);
            }
        }
        Debug.Log("You have " + bingoIDList.Count + " Bingos.");
    }

    // called when a new bingo is earned.  Made it a seperate function since I assume there are a lot of things we might want to trigger when this happens
    void GetBingo(int bingoId)
    {
        bingoIDList.Add(bingoId);
        Debug.Log("BINGO!");
        EndScreenUI.instance.WinScreen();
    }
}
