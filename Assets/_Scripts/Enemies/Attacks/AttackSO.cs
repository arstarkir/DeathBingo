using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public enum AttackType // enum for kind of attack, determines what it can overlap with
    {
        Primary, // can't be interrupted by another primary
        Modifier,
    }

    public GameObject attackPref;
    public AttackType attackType;
    [HideInInspector] public GameObject temp;

    public bool isAnimation = false;

    // For Time
    public bool isTimed = false;
    public float duration = 5;

    //For Random
    public bool isRandom = false;
    public GameObject toSpawn;
    public float attackAmount = 3;
    public float delay = 0.2f;

    public virtual void StartAttack(GameObject attackHolder)
    {
        if(temp == null)
            temp = Instantiate(attackPref, attackHolder.transform);

        if(isAnimation)
            temp.AddComponent<AnimationAttack>();
        if(isRandom)
            temp.AddComponent<RandomAttack>();
        if (isTimed)
            temp.AddComponent<TimedAttack>();

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