using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Singleton<Health>
{
    public int health = 3;
    [SerializeField] GameObject hpPref;
    [SerializeField] GameObject hpHolder;
    List<GameObject> curHealth = new List<GameObject>();
    Coroutine damagePoolTimer = null;
    List<DamageSource> damageSourcePool = new List<DamageSource>();

    [Tooltip("Player will be damaged only once in that time frame. But other damages will go to the rules")]
    public float damagePoolTime = 1;

    public override void Awake() 
    {
        base.Awake();
        OnHealthChange();
    }

    public int ChangeHealth(int changeAmount, DamageSource damageSource)
    {
        if (damagePoolTimer == null)
        {
            health += changeAmount;
            OnHealthChange();
            damagePoolTimer = StartCoroutine(DamageDealtPool());
        }
        damageSourcePool.Add(damageSource);
        BingoController.instance.RuleCheck(damageSourcePool);

        return health;
    }

    public void OnHealthChange()
    {
        foreach (GameObject healthObj in curHealth)
            Destroy(healthObj);
        curHealth.Clear();

        for (int i = 0; i < health; i++)
            curHealth.Add(Instantiate(hpPref, hpHolder.transform));
    }

    IEnumerator DamageDealtPool()
    {
        yield return new WaitForSeconds(damagePoolTime);
        damageSourcePool.Clear();
        damagePoolTimer = null;
    }
}
