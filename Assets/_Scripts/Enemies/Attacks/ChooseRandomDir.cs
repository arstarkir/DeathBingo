using UnityEngine;

public class ChooseRandomDir : MonoBehaviour
{
    public GameObject rotation;

    void Start()
    {
        float randomY = (float)(SeedManager.instance.rng.NextDouble() * 360.0);
        Vector3 euler = rotation.transform.eulerAngles;
        euler.y = randomY;
        rotation.transform.eulerAngles = euler;
    }
}
