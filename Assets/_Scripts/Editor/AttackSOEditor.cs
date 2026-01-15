using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackSO))]
public class AttackSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttackSO attack = (AttackSO)target;

        // stuff that should always be visible
        attack.attackPref = (GameObject)EditorGUILayout.ObjectField("Attack Prefab", attack.attackPref, typeof(GameObject), false); // attack prefab
        attack.attackType = (AttackSO.AttackType)EditorGUILayout.EnumPopup("Attack Type", attack.attackType); // attack type (primary/mod)
        attack.attackStyles = (AttackStyles)EditorGUILayout.EnumFlagsField("Attack Styles", attack.attackStyles); // style(s)

        // stuff that should sometimes be visible
        if (attack.attackStyles.HasFlag(AttackStyles.Timed)) // timed duration
        {
            attack.duration = EditorGUILayout.FloatField("Duration", attack.duration);
        }
        if (attack.attackStyles.HasFlag(AttackStyles.Randomed)) // random tospawn, amount, delay
        {
            attack.toSpawn = (GameObject)EditorGUILayout.ObjectField("To Spawn", attack.toSpawn, typeof(GameObject), false);
            attack.attackAmount = EditorGUILayout.FloatField("Attack Amount", attack.attackAmount);
            attack.delay = EditorGUILayout.FloatField("Delay", attack.delay);
        }
        if (attack.attackStyles.HasFlag(AttackStyles.ToPlayer)) // toPlayer player pos delay
        {
            attack.playerPosDelay = EditorGUILayout.FloatField("Player Pos Delay", attack.playerPosDelay);
        }
    }
}
