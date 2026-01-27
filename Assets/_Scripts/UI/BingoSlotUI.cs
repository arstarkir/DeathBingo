using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BingoSlotUI : MonoBehaviour
{
    public RuleSO rule;
    [SerializeField] TMP_Text text;
    [SerializeField] Toggle toggle;
    [SerializeField] Image previewVisual; // darken preview squares
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
    }

    public void SetAsPreview()
    {
        this.rule = null;
        text.text = "";
        finished = false;
        preview = true;
        toggle.isOn = false;
        previewVisual.enabled = true;
    }

    public void FinishRule()
    {
        if (preview) return;
        finished = true;
        toggle.isOn = finished;
    }
}
