using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSequence", menuName = "Scriptable Objects/AttackSequence")]
public class AttackSequenceSO : ScriptableObject
{
    [System.Serializable]
    public class AttackBlock // class to contain attack SO and specific wait time
    {
        public AttackSO attack;
        public float waitTime = -1;
        public Vector3 spawnLocation = Vector3.zero;
    }

    [Header("If waitTime is set to -1, Primary attacks will play once all previous primary attacks are complete.\nUnless specified in prior attack, Modifiers will always play immediately.\nWaitTime is how long it will take for the *current* attack to start!")]
    public List<AttackBlock> attacks = new List<AttackBlock>(); // list of attackblocks
}