using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public bool destroyOnDealDmg = false;
    public Damage damage;

    private void OnTriggerEnter(Collider other)
    {   
        if (!other.transform.root.gameObject.CompareTag("Player"))
            return;

        Health.instance.ChangeHealth(-1, damage);

        if (destroyOnDealDmg)
            Destroy(gameObject);
    }
}
