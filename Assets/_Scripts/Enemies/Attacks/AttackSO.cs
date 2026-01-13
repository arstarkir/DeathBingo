using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "SO/BasicAttack")]
public class AttackSO : ScriptableObject
{
    public GameObject attackPref;
    [HideInInspector] public GameObject temp;

    public virtual void StartAttack(GameObject attackHolder)
    {
        temp = Instantiate(attackPref, attackHolder.transform);
        temp.GetComponent<IAttackHandler>().attackSO = this;
    }

    public virtual void EndAttack()
    {
        if(temp != null)
            Destroy(temp);
        if (EnemyAttackSelection.instance != null)
        {
            EnemyAttackSelection.instance.isInAttack = false;
            return;
        }
        TestEnemyAttackSequencing.instance.isInAttack = false;
    }
}