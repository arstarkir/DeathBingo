using System.Collections.Generic;
using UnityEngine;

public class DarkCloudChance : MonoBehaviour
{
    [SerializeField] GameObject lightCloud; // visuals light cloud
    [SerializeField] GameObject darkCloud; // visuals dark cloud
    [SerializeField] float darkChance; // chance any given cloud is dark


    void Start()
    {
        if (SeedManager.instance.rng.NextDouble() < darkChance)
        {
            darkCloud.SetActive(true);
        }
        else
        {
            lightCloud.SetActive(true);
        }
    }

    public void CloudDeath()
    {
        double total = this.transform.parent.GetComponent<RandomAttack>().attackSO.attackAmount;
        Debug.Log(this.transform.GetSiblingIndex());
        if (total-1 <= this.transform.GetSiblingIndex())
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
