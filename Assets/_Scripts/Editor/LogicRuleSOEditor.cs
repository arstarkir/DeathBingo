using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LogicRuleSO))]
public class LogicRuleSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LogicRuleSO rule = (LogicRuleSO)target;
        rule.ruleFlag = (LogicRuleEnum)EditorGUILayout.EnumPopup("Rule Flag", rule.ruleFlag);  // enum picker for rule flag
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rules"), true); // list of rules

        if (rule.ruleFlag == LogicRuleEnum.AndAmount) // show 'and amount' field when flag is 'and amount'
        {
            rule.andAmount = EditorGUILayout.IntField("And Amount", rule.andAmount);
        }
        serializedObject.ApplyModifiedProperties(); // save list stuff
    }
}
