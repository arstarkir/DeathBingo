using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BingoSlotUI : MonoBehaviour
{
    public RuleSO rule;
    [SerializeField] TMP_Text text;
    [SerializeField] Toggle toggle;
    [SerializeField] Image previewVisual; // darken preview squares
    [SerializeField] Image singleSlot; // bingo image if single rule
    [SerializeField] Image doubleSlot1; // first bingo image if combo rule
    [SerializeField] Image doubleSlot2; // second bingo image if combo rule
    [SerializeField] Image orAnd; // or text

    [SerializeField] Sprite columnIcon; // these are sprites for the attack types (not specific attacks)
    [SerializeField] Sprite lightningIcon;
    [SerializeField] Sprite swordIcon;
    [SerializeField] Sprite tornadoIcon;
    [SerializeField] Sprite blueLightningIcon;
    [SerializeField] Sprite darkCloudIcon;

    [HideInInspector] public bool finished = false;
    [HideInInspector] public bool preview = false; // if true square is a preview of next bingo board size

    public void SetDamageRule(RuleSO damageRule)
    {
        this.rule = damageRule;
        text.text = damageRule.dataName;
        finished = false;
        preview = false;
        toggle.isOn = finished;
        previewVisual.enabled = false;
        orAnd.enabled = false;

        if ((damageRule is EffectRuleSO || damageRule is LocationRuleSO) && damageRule.trigger != DamageSource.Ignore) // location/effect rules with specified types need to also show the icon for that type
        {
            singleSlot.enabled = false;
            doubleSlot1.enabled = true;
            doubleSlot2.enabled = true;
            switch (damageRule.trigger)
            {
                case DamageSource.Column:
                    doubleSlot1.sprite = columnIcon;
                    break;
                case DamageSource.Lightning:
                    doubleSlot1.sprite = lightningIcon;
                    break;
                case DamageSource.Sword:
                    doubleSlot1.sprite = swordIcon;
                    break;
                case DamageSource.BlueLightning:
                    doubleSlot1.sprite = blueLightningIcon;
                    break;
                case DamageSource.Tornado:
                    doubleSlot1.sprite = tornadoIcon;
                    break;
                case DamageSource.DarkCloud:
                    doubleSlot1.sprite = darkCloudIcon;
                    break;
            }
            doubleSlot2.sprite = damageRule.bingoSprite;
            return;
        }

        if (damageRule.bingoSprite != null) // even if it's a combo rule, you can manually set a single image
        {
            singleSlot.enabled = true;
            doubleSlot1.enabled = false;
            doubleSlot2.enabled = false;
            singleSlot.sprite = damageRule.bingoSprite;
            return;
        }

        bool isLogicRule = damageRule is LogicRuleSO;
        singleSlot.enabled = !isLogicRule;
        doubleSlot1.enabled = isLogicRule;
        doubleSlot2.enabled = isLogicRule;

        if (isLogicRule)
        {
            LogicRuleSO logicRule = (LogicRuleSO)damageRule;
            orAnd.enabled = logicRule.ruleFlag == LogicRuleEnum.Or;
            if (logicRule.rules.Count == 2) // only 2 supported *for now*
            {
                if (logicRule.rules[0].bingoSprite != null)
                {
                    doubleSlot1.sprite = logicRule.rules[0].bingoSprite;
                }
                if (logicRule.rules[1].bingoSprite != null)
                {
                    doubleSlot2.sprite = logicRule.rules[1].bingoSprite;
                }
            }
        }
    }

    public void SetAsPreview()
    {
        this.rule = null;
        text.text = "";
        finished = false;
        preview = true;
        toggle.isOn = false;
        previewVisual.enabled = true;
        singleSlot.enabled = false;
        doubleSlot1.enabled = false;
        orAnd.enabled = false;
        doubleSlot2.enabled = false;
    }

    public void FinishRule()
    {
        if (preview) return;
        finished = true;
        toggle.isOn = finished;
    }
}
