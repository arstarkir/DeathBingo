using System.Linq;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public bool destroyOnDealDmg = false;
    public DamageSource damageSource;

    private void OnTriggerEnter(Collider other)
    {   
        if (!other.gameObject.CompareTag("Player"))
            return;

        Health.instance.ChangeHealth(-1, damageSource, transform.root.GetComponentsInChildren<IAttackHandler>().First());

        if (destroyOnDealDmg)
            Destroy(gameObject);
    }
}
