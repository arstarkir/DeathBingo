using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LogicRuleSO))]
public class LogicRuleSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (target == null) return;
        serializedObject.UpdateIfRequiredOrScript();
        LogicRuleSO rule = (LogicRuleSO)target;
        if (rule == null) return;

        EditorGUI.BeginChangeCheck();
        Undo.RecordObject(rule, "Edit LogicRuleSO");

        rule.dataName = EditorGUILayout.TextField("Rule Name", rule.dataName);
        rule.dataDescription = EditorGUILayout.TextField("Rule Hover Text", rule.dataDescription);

        rule.ruleFlag = (LogicRuleEnum)EditorGUILayout.EnumPopup("Rule Flag", rule.ruleFlag);  // enum picker for rule flag
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rules"), true); // list of rules

        if (rule.ruleFlag == LogicRuleEnum.AndAmount) // show 'and amount' field when flag is 'and amount'
        {
            rule.andAmount = EditorGUILayout.IntField("And Amount", rule.andAmount);
        }

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(rule);

        serializedObject.ApplyModifiedProperties();
    }
}
