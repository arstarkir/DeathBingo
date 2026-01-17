using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BingoSlotUI : MonoBehaviour
{
    public RuleSO rule;
    [SerializeField] TMP_Text text;
    [SerializeField] Toggle toggle;
    [HideInInspector] public bool finished = false;

    public void SetDamageRule(RuleSO damageRule)
    {
        this.rule = damageRule;
        text.text = damageRule.dataName;
        finished = false;
        toggle.isOn = finished;
    }

    public void FinishRule()
    {
        finished = true;
        toggle.isOn = finished;
    }
}
