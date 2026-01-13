using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "SO/BasicAttack")]
public class AttackSO : ScriptableObject
{
    public GameObject attackPref;
    GameObject temp;

    public virtual void StartAttack(GameObject attackHolder)
    {
        temp = Instantiate(attackPref, attackHolder.transform);
        temp.GetComponent<IAttackHandler>().attackSO = this;
    }

    public virtual void EndAttack()
    {
        if(temp != null)
            Destroy(temp);
        TestEnemyAttackSequencing.instance.isInAttack = false;
    }
}