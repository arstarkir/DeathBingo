using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindControlls : Singleton<RebindControlls>
{
    public InputActionAsset actions;

    TMP_Text bindingText;

    InputAction action;
    InputActionRebindingExtensions.RebindingOperation rebindOp;

    public void SetBindingText(TMP_Text text)
    {
        bindingText = text;
    }

    public void SetAction(string actionMapName, string actionName)
    {
        action = actions.FindActionMap(actionMapName).FindAction(actionName);
    }

    public void StartRebind(int bindingIndex)
    {
        action.Disable();
        bindingText.alpha = 0.5f;

        rebindOp = action.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .OnCancel(op =>
            {
                Cleanup(bindingIndex);
                action.Enable();
                op.Dispose();
                rebindOp = null;
            })
            .OnComplete(op =>
            {
                Cleanup(bindingIndex);
                action.Enable();
                op.Dispose();
                rebindOp = null;
            });

        rebindOp.Start();
    }

    public void StartRebindCompositePart(string partName)
    {
        int bindingIndex = FindCompositePartBindingIndex(action, partName);
        StartRebind(bindingIndex);
    }

    public void UpdateTextForCompositePart(string partName)
    {
        int bindingIndex = FindCompositePartBindingIndex(action, partName);
        bindingText.text = action.GetBindingDisplayString(
            bindingIndex, out _, out _,
            InputBinding.DisplayStringOptions.DontUseShortDisplayNames
        );
    }

    void Cleanup(int bindingIndex)
    {
        bindingText.alpha = 1f;
        bindingText.text = action.GetBindingDisplayString(
            bindingIndex, out _, out _,
            InputBinding.DisplayStringOptions.DontUseShortDisplayNames
        );
    }

    private void OnDisable()
    {
        rebindOp?.Dispose();
        rebindOp = null;
    }

    static int FindCompositePartBindingIndex(InputAction a, string partName)
    {
        for (int i = 0; i < a.bindings.Count; i++)
        {
            var b = a.bindings[i];
            if (b.isPartOfComposite && b.name == partName)
                return i;
        }
        return -1;
    }
}