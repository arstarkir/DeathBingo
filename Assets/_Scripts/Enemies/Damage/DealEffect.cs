using UnityEngine;

public class DealEffect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {   
        if (!other.transform.root.gameObject.CompareTag("Player") || !this.enabled)
            return;

        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.root.gameObject.CompareTag("Player") || !this.enabled)
            return;

        Destroy(gameObject);
    }
}
