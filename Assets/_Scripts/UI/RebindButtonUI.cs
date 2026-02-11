using TMPro;
using UnityEngine;

public class RebindButtonUI : MonoBehaviour
{
    public string actionMap = "Player";
    public string actionName = "Move";
    public string compositePart;
    public int bindingIndex = -1;
    public TMP_Text bindingText;

    public void Rebind()
    {
        RebindControlls.instance.SetAction(actionMap, actionName);
        RebindControlls.instance.SetBindingText(bindingText);
        if(bindingIndex != -1)
            RebindControlls.instance.StartRebind(bindingIndex);
        else
            RebindControlls.instance.StartRebindCompositePart(compositePart);
    }
}