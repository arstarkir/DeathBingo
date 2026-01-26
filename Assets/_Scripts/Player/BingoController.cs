using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class BingoController : Singleton<BingoController>
{
    public int boardSize; // dimension of bingo board, 1-5

    public bool randomizeRuleRelocation; // if true, rules will be placed in random spots when the board expands

    public GameObject slotPref;
    public GameObject slotHolder;

    public List<RuleSO> activeRules = new List<RuleSO>();
    List<RuleSO> finishedRules = new List<RuleSO>();
    List<BingoSlotUI> curSlots = new List<BingoSlotUI>();

    int[][] validBingos; // current active bingo lines
    int[][] validBingos5 = // list of valid bingo lines for 5x5
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
    int[][] validBingos4 = // list of valid bingo lines for 4x4
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
    int[][] validBingos3 = // list of valid bingo lines for 3x3
    {
        // rows
        new int[] { 0, 1, 2 },
        new int[] { 3, 4, 5 },
        new int[] { 6, 7, 8 },

        // columns
        new int[] { 0, 3, 6 },
        new int[] { 1, 4, 7 },
        new int[] { 2, 5, 8 },

        // diagonals
        new int[] { 0, 4, 8 },
        new int[] { 2, 4, 6 }
    };
    int[][] validBingos2 = // list of valid bingo lines for 2x2
    {
        // rows
        new int[] { 0, 1, },
        new int[] { 2, 3, },

        // columns
        new int[] { 0, 2, },
        new int[] { 1, 3, },

        // diagonals
        new int[] { 0, 3 },
        new int[] { 2, 1 }
    };
    int[][] validBingos1 = { new int[] { 0 } }; // valid bingo 1x1


    public List<string> bingoIDList = new List<string>(); // list of ID of Bingos achieved (Currently broken, WIP)

    [HideInInspector] public int maxRuleCombo = 0;

    public void Start()
    {
        boardSize = 0;
        foreach (var slot in curSlots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        curSlots.Clear();
        activeRules.Clear();
    }

    // set size of bingo board
    public void SetBoardSize(int newSize, List<RuleGroupSO> newGroups)
    {
        newSize = Mathf.Clamp(newSize, 1, 5);
        List<RuleSO> oldRules = curSlots.Select(s => s.rule).ToList();
        HashSet<RuleSO> finishedRulesSet = new HashSet<RuleSO>(curSlots.Where(s => s.finished).Select(s => s.rule));

        foreach (var slot in curSlots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        curSlots.Clear();
        activeRules.Clear();

        GridLayoutGroup grid = slotHolder.GetComponent<GridLayoutGroup>(); // resize bingo board grid
        if (grid != null)
        {
            float totalSpacingX = grid.spacing.x * (newSize - 1);
            float totalSpacingY = grid.spacing.y * (newSize - 1);
            RectTransform rt = slotHolder.GetComponent<RectTransform>();
            float availableWidth = rt.rect.width - grid.padding.left - grid.padding.right - totalSpacingX;
            float availableHeight = rt.rect.height - grid.padding.top - grid.padding.bottom - totalSpacingY;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = newSize;
            grid.cellSize = new Vector2(availableWidth / newSize, availableHeight / newSize);
        }

        List<RuleSO> newRules = ConvertBoard(boardSize, newSize, oldRules); // put old rules onto new board
        boardSize = newSize;

        Dictionary<RuleGroupSO, int> groupCounts = newGroups.ToDictionary(g => g, g => 0); // generate the rest of the rules
        HashSet<RuleSO> usedRules = new HashSet<RuleSO>(oldRules);

        for (int i = 0; i < newSize * newSize; i++) // most of this is copied over from the old rulegroup function
        {
            if (newRules[i] != null) continue;

            bool slotFilled = false;

            while (!slotFilled)
            {
                var validGroups = newGroups.Where(g => groupCounts[g] < g.maxSelected).ToList();

                if (validGroups.Count == 0)
                {
                    foreach (var key in groupCounts.Keys.ToList()) groupCounts[key] = 0;
                    usedRules.Clear();
                    validGroups = new List<RuleGroupSO>(newGroups);
                }

                RuleGroupSO group = validGroups[SeedManager.instance.rng.Next(validGroups.Count)];
                var validRules = group.rules.Where(r => !usedRules.Contains(r)).ToList();

                if (validRules.Count == 0)
                {
                    validRules = group.rules.ToList();
                }

                RuleSO rule = validRules[SeedManager.instance.rng.Next(validRules.Count)];
                newRules[i] = rule;
                usedRules.Add(rule);
                groupCounts[group]++;
                slotFilled = true;
            }
        }

        foreach (RuleSO rule in newRules)
        {
            activeRules.Add(rule);
            BingoSlotUI temp = Instantiate(slotPref, slotHolder.transform).GetComponent<BingoSlotUI>();
            temp.SetDamageRule(rule);
            if (finishedRulesSet.Contains(rule)) // marking old completed rules as completed
            {
                temp.FinishRule();
                activeRules.Remove(rule);
            }
            curSlots.Add(temp);
        }

        switch (newSize)
        {
            case 1: validBingos = validBingos1; break;
            case 2: validBingos = validBingos2; break;
            case 3: validBingos = validBingos3; break;
            case 4: validBingos = validBingos4; break;
            case 5: validBingos = validBingos5; break;
        }
        bingoIDList.Clear();
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
            if (bingoIDList.Contains(bingoId + "board" + boardSize)) continue;
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

    // this is nearly identical to the bingo function, but it doesn't reward Bingos, just checks for them to prevent Bingos being randomly generated
    public bool PreventBingo()
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
            if (bingoIDList.Contains(bingoId + "board" + boardSize)) continue;
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
                return true;
            }
        }
        return false;
    }

    // called when a new bingo is earned.  Made it a seperate function since I assume there are a lot of things we might want to trigger when this happens
    void GetBingo(int bingoId)
    {
        bingoIDList.Add(bingoId + "board" + boardSize);
        Debug.Log("BINGO!");
        EnemyAttackSelection.instance.EndWave();
    }

    // move old rules into the correct place in a larger list of rules
    List<RuleSO> ConvertBoard(int oldSize, int newSize, List<RuleSO> oldRules)
    {
        List<RuleSO> newRules = Enumerable.Repeat<RuleSO>(null, newSize * newSize).ToList();
        if (oldRules == null || oldRules.Count == 0)
        {
            return newRules;
        }
        if (randomizeRuleRelocation)
        {
            List<int> usedIds = new List<int>();
            foreach (RuleSO rule in oldRules)
            {
                int newId = 0;
                newId = SeedManager.instance.rng.Next(newSize * newSize);
                newRules[newId] = rule;
                while (PreventBingo() && !usedIds.Contains(newId))
                {
                    newId = SeedManager.instance.rng.Next(newSize * newSize);
                    newRules[newId] = rule;
                }
                usedIds.Add(newId);
            }
        }
        else if (oldSize < 2 && newSize == 2) // converting a 1x1 to a 2x2
        {
            newRules[0] = oldRules[0];
        }
        else if (oldSize < 3 && newSize == 3) // converting a 2x2 to a 3x3
        {
            newRules[4] = oldRules[0];
            newRules[5] = oldRules[1];
            newRules[7] = oldRules[2];
            newRules[8] = oldRules[3];
        }
        else if (oldSize < 4 && newSize == 4) // converting a 3x3 to a 4x4
        {
            newRules[5] = oldRules[0];
            newRules[6] = oldRules[1];
            newRules[7] = oldRules[2];
            newRules[9] = oldRules[3];
            newRules[10] = oldRules[4];
            newRules[11] = oldRules[5];
            newRules[13] = oldRules[6];
            newRules[14] = oldRules[7];
            newRules[15] = oldRules[8];
        }
        else if (oldSize < 5 && newSize == 5) // converting a 4x4 to a 5x5
        {
            newRules[0] = oldRules[0];
            newRules[1] = oldRules[1];
            newRules[2] = oldRules[2];
            newRules[3] = oldRules[3];
            newRules[5] = oldRules[4];
            newRules[6] = oldRules[5];
            newRules[7] = oldRules[6];
            newRules[8] = oldRules[7];
            newRules[10] = oldRules[8];
            newRules[11] = oldRules[9];
            newRules[12] = oldRules[10];
            newRules[13] = oldRules[11];
            newRules[15] = oldRules[12];
            newRules[16] = oldRules[13];
            newRules[17] = oldRules[14];
            newRules[18] = oldRules[15];
        }
        return newRules;
    }
}
