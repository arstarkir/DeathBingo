using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "SO/BasicAttack")]
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

    public virtual void StartAttack(GameObject attackHolder)
    {
        temp = Instantiate(attackPref, attackHolder.transform);
        temp.GetComponent<IAttackHandler>().attackSO = this;
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