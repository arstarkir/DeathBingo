using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : Singleton<Health>
{
    public int health = 3;
    [SerializeField] GameObject hpPref;
    [SerializeField] GameObject hpHolder;
    List<GameObject> curHealth = new List<GameObject>();
    Coroutine damagePoolTimer = null;
    List<Damage> damagePool = new List<Damage>();

    [Tooltip("Player will be damaged only once in that time frame. But other damages will go to the rules")]
    public float damagePullTime = 1;

    public override void Awake() 
    {
        base.Awake();
        OnHealthChange();
    }

    public int ChangeHealth(int changeAmount, Damage damage)
    {
        if (damagePoolTimer == null)
        {
            health += changeAmount;
            OnHealthChange();
            damagePoolTimer = StartCoroutine(DamageDealtPool());
        }
        damagePool.Add(damage);
        RuleCheck();

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
        yield return new WaitForSeconds(damagePullTime);
        damagePool.Clear();
        damagePoolTimer = null;
    }

    public void RuleCheck() // Might change it to be sent to the Bingo Bored Controller
    {
        foreach(Damage damage in damagePool)
        {
            // Check rules here
        }
    }
}
