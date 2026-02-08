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

    [SerializeField] Sprite columnIcon; // these are sprites for the attack types (not specific attacks)
    [SerializeField] Sprite lightningIcon;
    [SerializeField] Sprite swordIcon;

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

        if (damageRule is EffectRuleSO effectRule && effectRule.trigger != DamageSource.Ignore) // if it's an effect rule with specified type (ex: wet + columns) use the type icon
        {
            singleSlot.enabled = false;
            doubleSlot1.enabled = true;
            doubleSlot2.enabled = true;
            switch (effectRule.trigger)
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
            }
            doubleSlot2.sprite = effectRule.bingoSprite;
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
        doubleSlot2.enabled = false;
    }

    public void FinishRule()
    {
        if (preview) return;
        finished = true;
        toggle.isOn = finished;
    }
}
