using UnityEngine;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public int health = 3;
    [SerializeField] GameObject hpPref;
    [SerializeField] GameObject hpHolder;
    List<GameObject> curHealth = new List<GameObject>();

    public void Awake()
    {
        OnHealthChange();
    }

    public int ChangeHealth(int changeAmount)
    {
        health += changeAmount;
        OnHealthChange();
        return health;
    }

    public void OnHealthChange()
    {
        foreach (GameObject health in curHealth)
        {
            curHealth.Remove(health);
            Destroy(health);
        }

        for (int i = 0; i < health; i++)
            curHealth.Add(Instantiate(hpPref, hpHolder.transform));
    }
}
