using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BingoSlotUI : MonoBehaviour
{
    public DamageRuleSO rule;
    [SerializeField] TMP_Text text;
    [SerializeField] Toggle toggle;
    [HideInInspector] public bool finished = false;

    public void SetDamageRule(DamageRuleSO damageRule)
    {
        this.rule = damageRule;
        text.text = damageRule.ruleName;
        finished = false;
        toggle.isOn = finished;
    }

    public void FinishRule()
    {
        finished = true;
        toggle.isOn = finished;
    }
}
