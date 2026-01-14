using System.Collections;
using UnityEngine;

public class TimedAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }
    
    void Start()
    {
        StartCoroutine(EndAfterTime(attackSO.duration, attackSO));
    }

    public IEnumerator EndAfterTime(float s, AttackSO attack)
    {
        yield return new WaitForSeconds(s);
        attack.EndAttack();
    }
}