using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BingoController : Singleton<BingoController>
{
    public GameObject slotPref;
    public GameObject slotHolder;

    public List<DamageRuleSO> activeRules = new List<DamageRuleSO>();
    List<DamageRuleSO> finishedRules = new List<DamageRuleSO>();
    List<BingoSlotUI> curSlots = new List<BingoSlotUI>();

    public override void Awake()
    {
        base.Awake();
        foreach (DamageRuleSO rule in activeRules)
        {
            BingoSlotUI temp = Instantiate(slotPref, slotHolder.transform).GetComponent<BingoSlotUI>();
            temp.SetDamageRule(rule);
            curSlots.Add(temp);
        }
    }

    public void RuleCheck(List<DamageSource> damageSources)
    {
        foreach (DamageSource source in damageSources) // this will be a max of 3 so it should be fine
        {
            foreach(DamageRuleSO rule in activeRules)
            {
                if(rule.CheckRule(source))
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
    }
}
