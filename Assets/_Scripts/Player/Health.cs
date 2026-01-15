using Microsoft.Win32.SafeHandles;
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
    List<(DamageSource, IAttackHandler)> damageInfoPool = new List<(DamageSource, IAttackHandler)>();

    [SerializeField] float respawnTimer = 3f; // seconds before respawn on death
    [SerializeField] GameObject playerCollider; // collider to disable while dead
    [SerializeField] float invincibilityTime = 3f;   // seconds of invincibiilty on respawn

    [Tooltip("Player will be damaged only once in that time frame. But other damages will go to the rules")]
    public float damagePoolTime = 1;

    public override void Awake() 
    {
        base.Awake();
        OnHealthChange();
    }

    public int ChangeHealth(int changeAmount, DamageSource damageSource, IAttackHandler handler = null)
    {
        if (damagePoolTimer == null)
        {
            health += changeAmount;
            OnHealthChange();
            damagePoolTimer = StartCoroutine(DamageDealtPool(damagePoolTime));
        }
        damageInfoPool.Add((damageSource, handler));
        BingoController.instance.RuleCheck(damageInfoPool);

        return health;
    }

    public void OnHealthChange()
    {
        foreach (GameObject healthObj in curHealth)
            Destroy(healthObj);
        curHealth.Clear();

        for (int i = 0; i < health; i++)
            curHealth.Add(Instantiate(hpPref, hpHolder.transform));
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageDealtPool(float duration)
    {
        yield return new WaitForSeconds(damagePoolTime);
        damageInfoPool.Clear();
        damagePoolTimer = null;
    }

    // hide, reset, and start couroutine for respawn when player dies
    public void Die()
    {
        Rigidbody rb = playerCollider.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        playerCollider.SetActive(false);
        StartCoroutine(Respawn());
    }

    // routine for waiting to respawn and respawning
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTimer);
        playerCollider.SetActive(true);
        playerCollider.transform.position = Vector3.zero;
        health = 3;
        OnHealthChange();
        damagePoolTimer = StartCoroutine(DamageDealtPool(invincibilityTime));
    }
}
