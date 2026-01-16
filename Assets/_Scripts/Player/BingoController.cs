using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class BingoController : Singleton<BingoController>
{
    public GameObject slotPref;
    public GameObject slotHolder;

    public List<DamageRuleSO> activeRules = new List<DamageRuleSO>();
    List<DamageRuleSO> finishedRules = new List<DamageRuleSO>();
    List<BingoSlotUI> curSlots = new List<BingoSlotUI>();   
    int[][] validBingos = // list of valid bingo lines for 4x4
    {
        // rows
        new int[] { 0, 1, 2, 3 },
        new int[] { 4, 5, 6, 7 },
        new int[] { 8, 9, 10, 11 },
        new int[] { 12, 13, 14, 15 },

        // columns
        new int[] { 0, 4, 8, 12 },
        new int[] { 1, 5, 9, 13 },
        new int[] { 2, 6, 10, 14 },
        new int[] { 3, 7, 11, 15 },

        // diagonals
        new int[] { 0, 5, 10, 15 },
        new int[] { 3, 6, 9, 12 }   
    };

    List<int> bingoIDList = new List<int>(); // list of ID of Bingos achieved (.Count to see how many Bingos a player has)

    public override void Awake()
    {
        base.Awake();
        Shuffle(activeRules, SeedManager.instance.rng);
        foreach (DamageRuleSO rule in activeRules)
        {
            BingoSlotUI temp = Instantiate(slotPref, slotHolder.transform).GetComponent<BingoSlotUI>();
            temp.SetDamageRule(rule);
            curSlots.Add(temp);
        }
    }

    // put bingo squares in random order and cap at 16
    void Shuffle<DamageRuleSO>(List<DamageRuleSO> list, System.Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        if (list.Count > 16)
        {
            list.RemoveRange(16, list.Count - 16);
        }
    }

    public void RuleCheck(List<(DamageSource, IAttackHandler)> damageInfo)
    {
        foreach ((DamageSource, IAttackHandler) info in damageInfo) // this will be a max of 3 so it should be fine
        {
            foreach(DamageRuleSO rule in activeRules)
            {
                if(rule.CheckRule(info))
                    finishedRules.Add(rule);
            }
        }

        if (finishedRules.Count > 0)
            FinishRules();
    }

    public void FinishRules()
    {
        List<BingoSlotUI> active = curSlots.Where(slot => !slot.finished).ToList();

        foreach (DamageRuleSO rule in finishedRules)
        {
            active.Find(slot => slot.rule == rule).FinishRule();
            activeRules.Remove(rule);
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
    }
}
