using UnityEngine;

public class Push : MonoBehaviour
{
    public Vector3 localDir = Vector3.forward;
    public float force = 10f;
    bool isPushing = false;
    CharacterController player;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        player = other.GetComponent<CharacterController>();
        player.pushVelocity += localDir * force;
        isPushing = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        player.pushVelocity -= localDir * force;
        isPushing = false;
    }

    private void OnDestroy()
    {
        if(isPushing)
            player.pushVelocity -= localDir * force;
    }
}