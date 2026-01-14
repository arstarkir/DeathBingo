using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TimedAttack", menuName = "Attack/TimedAttack")]
public class TimedAttackSO : AttackSO
{
    public float duration = 3;

    public override void StartAttack(GameObject attackHolder)
    {
        temp = Instantiate(attackPref, attackHolder.transform);
        temp.AddComponent<TimedAttack>();
        temp.GetComponent<IAttackHandler>().attackSO = this;
    }

    public IEnumerator EndAfterTime(float s)
    {
        yield return new WaitForSeconds(s);
        EndAttack();
    }

    public override void EndAttack()
    {
        base.EndAttack();
    }
}

public class TimedAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }
    
    void Start()
    {
        TimedAttackSO ta = (TimedAttackSO)attackSO;

        StartCoroutine(ta.EndAfterTime(ta.duration));
    }
}