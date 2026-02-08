using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMovingRuleSO))]
public class PlayerMovingRuleSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (target == null) return;
        serializedObject.UpdateIfRequiredOrScript();
        PlayerMovingRuleSO rule = (PlayerMovingRuleSO)target;
        if (rule == null) return;

        EditorGUI.BeginChangeCheck();
        Undo.RecordObject(rule, "Edit PlayerMovingRuleSO");

        DrawPropertiesExcluding( // things like name and desc from attackSO
            serializedObject,
            "moveVector",
            "axis",
            "logic",
            "tolerance"
        );

        // stuff always visible
        rule.moveVector = EditorGUILayout.Vector3Field("Move Vector", rule.moveVector); // move vector
        rule.axis = (Axis)EditorGUILayout.EnumFlagsField("Axis", rule.axis); // axis enum picker
        rule.logic = (AxisLogic)EditorGUILayout.EnumPopup("Logic", rule.logic); // logic enum picker

        // stuff hidden if not == or !=
        if ((rule.logic == AxisLogic.Equal) ||( rule.logic == AxisLogic.NotEqual))
        {
            rule.tolerance = EditorGUILayout.FloatField("Tolerance", rule.tolerance); // tolerance range
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(rule);
            AssetDatabase.SaveAssets();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
