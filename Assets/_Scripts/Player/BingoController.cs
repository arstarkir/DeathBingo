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
    public bool showPreview = true; // if true, shows preview of next board size

    public GameObject slotPref;
    public GameObject slotHolder;

    public List<RuleSO> activeRules = new List<RuleSO>();
    List<RuleSO> finishedRules = new List<RuleSO>();
    List<BingoSlotUI> curSlots = new List<BingoSlotUI>();
    List<bool> actualSlotFinished = new List<bool>(); // keeps track of finished slots on board

    int[][] validBingos; // current active bingo lines
    #region Valid Bingo lists
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
    #endregion


    public List<string> bingoIDList = new List<string>(); // list of ID of Bingos achieved

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

    // basically take in # of slot on board including preview slots, get back # it actually represents in that board (-1 if it isn't in it)
    // ex: my actual bingo board is 2x2, but the preview slots make it a 3x3.  Send in what # a slot is in a 3x3 and get back what it actually is in the 2x2.
    // this is only relevant if you have preview on
    int GetRealSlotNum(int visualIndex, int currentSize, int previewSize)
    {
        if (currentSize == 1 && previewSize == 2) // 1x1 goes in top left
        {
            return visualIndex == 0 ? 0 : -1;
        }
        else if (currentSize == 2 && previewSize == 3) // 2x2 goes in bottom right
        {
            switch (visualIndex)
            {
                case 4: return 0;
                case 5: return 1;
                case 7: return 2;
                case 8: return 3;
                default: return -1;
            }
        }
        else if (currentSize == 3 && previewSize == 4) // 3x3 goes in bottom left
        {
            switch (visualIndex)
            {
                case 5: return 0;
                case 6: return 1;
                case 7: return 2;
                case 9: return 3;
                case 10: return 4;
                case 11: return 5;
                case 13: return 6;
                case 14: return 7;
                case 15: return 8;
                default: return -1;
            }
        }
        else if (currentSize == 4 && previewSize == 5) // 4x4 goes in top left
        {
            switch (visualIndex)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 5: return 4;
                case 6: return 5;
                case 7: return 6;
                case 8: return 7;
                case 10: return 8;
                case 11: return 9;
                case 12: return 10;
                case 13: return 11;
                case 15: return 12;
                case 16: return 13;
                case 17: return 14;
                case 18: return 15;
                default: return -1;
            }
        }
        else if (currentSize == 5 && previewSize == 5) // 5x5 doesn't need shuffling THANK GOD
        {
            return visualIndex < 25 ? visualIndex : -1;
        }
        return -1;
    }

    // set size of bingo board
    public void SetBoardSize(int newSize, List<RuleGroupSO> newGroups)
    {
        newSize = Mathf.Clamp(newSize, 1, 5);
        List<RuleSO> oldRules = curSlots.Where(s => !s.preview && s.rule != null).Select(s => s.rule).ToList();
        HashSet<RuleSO> finishedRulesSet = new HashSet<RuleSO>(curSlots.Where(s => s.finished && !s.preview).Select(s => s.rule));

        foreach (var slot in curSlots) // wipe everything
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        curSlots.Clear();
        activeRules.Clear();

        int displaySize = showPreview ? Mathf.Min(newSize + 1, 5) : newSize; // if preview is on, increase visual size by 1
        GridLayoutGroup grid = slotHolder.GetComponent<GridLayoutGroup>(); // resize bingo board grid
        if (grid != null)
        {
            float totalSpacingX = grid.spacing.x * (displaySize - 1);
            float totalSpacingY = grid.spacing.y * (displaySize - 1);
            RectTransform rt = slotHolder.GetComponent<RectTransform>();
            float availableWidth = rt.rect.width - grid.padding.left - grid.padding.right - totalSpacingX;
            float availableHeight = rt.rect.height - grid.padding.top - grid.padding.bottom - totalSpacingY;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = displaySize;
            grid.cellSize = new Vector2(availableWidth / displaySize, availableHeight / displaySize);
        }

        List<RuleSO> newRules = ConvertBoard(boardSize, newSize, oldRules); // put old rules onto new board
        boardSize = newSize;

        Dictionary<RuleGroupSO, int> groupCounts = newGroups.ToDictionary(g => g, g => 0); // generate the rest of the rules
        HashSet<RuleSO> usedRules = new HashSet<RuleSO>(oldRules.Where(r => r != null));

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

        actualSlotFinished = Enumerable.Repeat(false, newSize * newSize).ToList(); // set all slots to not finished

        for (int i = 0; i < displaySize * displaySize; i++) // looping through current bingo board to mark things as finished, unfinished, or preview slots
        {
            BingoSlotUI temp = Instantiate(slotPref, slotHolder.transform).GetComponent<BingoSlotUI>();
            int logicalSlot = showPreview ? GetRealSlotNum(i, newSize, displaySize) : i; // if preview is on, you have to convert visual slot id to the actual one
            if (logicalSlot >= 0 && logicalSlot < newRules.Count && newRules[logicalSlot] != null)
            {
                RuleSO rule = newRules[logicalSlot];
                activeRules.Add(rule);
                temp.SetDamageRule(rule);
                if (finishedRulesSet.Contains(rule)) // marking old completed rules as completed
                {
                    temp.FinishRule();
                    activeRules.Remove(rule);
                    actualSlotFinished[logicalSlot] = true;
                }
            }
            else
            {
                temp.SetAsPreview();
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
        List<BingoSlotUI> active = curSlots.Where(slot => !slot.finished && slot.rule != null).ToList();

        foreach (RuleSO rule in finishedRules)
        {
            int actualIndex = 0;
            for (int i = 0; i < curSlots.Count; i++)
            {
                if (!curSlots[i].preview && curSlots[i].rule != null)
                {
                    if (curSlots[i].rule == rule)
                    {
                        curSlots[i].FinishRule();
                        if (actualIndex < actualSlotFinished.Count)
                        {
                            actualSlotFinished[actualIndex] = true;
                        }
                    }
                    actualIndex++;
                }
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
        for (int i = 0; i < actualSlotFinished.Count; i++) // list of id of squares earned
        {
            if (actualSlotFinished[i])
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
    public bool PreventBingo(List<bool> slotFinished, int[][] bingos, int targetBoardSize)
    {
        HashSet<int> finishedSlots = new HashSet<int>();
        for (int i = 0; i < slotFinished.Count; i++)
        {
            if (slotFinished[i])
            {
                finishedSlots.Add(i);
            }
        }
        for (int bingoId = 0; bingoId < bingos.Length; bingoId++)
        {
            if (bingoIDList.Contains(bingoId + "board" + targetBoardSize)) continue;
            bool isBingo = true;
            foreach (int slot in bingos[bingoId])
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
        if (randomizeRuleRelocation) // if it's random the only constraint is not making a bingo by accident
        {
            int[][] tempValidBingos = null;
            switch (newSize)
            {
                case 1: tempValidBingos = validBingos1; break;
                case 2: tempValidBingos = validBingos2; break;
                case 3: tempValidBingos = validBingos3; break;
                case 4: tempValidBingos = validBingos4; break;
                case 5: tempValidBingos = validBingos5; break;
            }
            List<bool> tempSlotFinished = Enumerable.Repeat(false, newSize * newSize).ToList();
            List<int> usedIds = new List<int>();
            HashSet<RuleSO> finishedOldRules = new HashSet<RuleSO>(curSlots.Where(s => s.finished && !s.preview && s.rule != null).Select(s => s.rule));
            foreach (RuleSO rule in oldRules) // place each rule into the new board
            {
                bool valid = false;
                int newId = -1;
                while (!valid)
                {
                    newId = SeedManager.instance.rng.Next(newSize * newSize);
                    if (usedIds.Contains(newId))
                    {
                        continue;
                    }
                    if (finishedOldRules.Contains(rule))
                    {
                        tempSlotFinished[newId] = true;
                        if (PreventBingo(tempSlotFinished, tempValidBingos, newSize)) // if a finished rule placement makes a bingo it's a bad placement bozo
                        {
                            tempSlotFinished[newId] = false;
                            continue;
                        }
                    }
                    valid = true;
                }
                newRules[newId] = rule;
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
