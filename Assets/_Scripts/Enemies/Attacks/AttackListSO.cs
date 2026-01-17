using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackList", menuName = "SOList/AttackList")]
public class AttackListSO : ScriptableObject
{
    public List<AttackSO> attacks = new List<AttackSO>();

    public int GetAttackId(AttackSO attack)
    {
        int id = attacks.FindIndex(a => a.dataName == attack.dataName);
        return id;
    }

    public int GetAttackIdByName(string attackName)
    {
        int id = attacks.FindIndex(a => a.dataName == attackName);
        return id;
    }
}