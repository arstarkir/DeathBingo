using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attack")]
public class AttackSO : Data
{
    public enum AttackType // enum for kind of attack, determines what it can overlap with
    {
        Primary, // can't be interrupted by another primary
        Modifier,
    }

    public GameObject attackPref;
    public AttackType attackType;
    [HideInInspector] public GameObject temp;

    public AttackStyles attackStyles;

    // For Time
    public float duration = 5;  

    //For Random
    public GameObject toSpawn;
    public float attackAmount = 3;
    public float delay = 0.2f;

    //For ToPlayer
    public float playerPosDelay = 0.2f;


    public virtual void StartAttack(GameObject attackHolder)
    {
        dataType = DataType.Attack;
        if (temp == null)
            temp = Instantiate(attackPref, attackHolder.transform);

        if(attackStyles.HasFlag(AttackStyles.Randomed))
            temp.AddComponent<RandomAttack>();
        if (attackStyles.HasFlag(AttackStyles.Timed))
            temp.AddComponent<TimedAttack>();
        if (attackStyles.HasFlag(AttackStyles.ToPlayer))
            temp.AddComponent<ToPlayerAttack>();

        temp.GetComponents<IAttackHandler>().ToList().ForEach(attackHandler => attackHandler.attackSO = this);
    }

    public virtual void EndAttack()
    {
        if (temp != null)
            Destroy(temp);
        if (attackType == AttackType.Primary)
        {
            if (EnemyAttackSelection.instance != null)
            {
                EnemyAttackSelection.instance.isInAttack = false;
                return;
            }
            TestEnemyAttackSequencing.instance.isInAttack = false;
        }
    }
}

[Flags]
public enum AttackStyles
{
    None = 0,
    Animation = 1 << 0,
    Timed = 1 << 1,
    Randomed = 1 << 2,
    ToPlayer = 1 << 3,
    All = Animation | Timed | Randomed | ToPlayer,
}