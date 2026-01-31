using UnityEngine;

public class TargetPlayerZ : MonoBehaviour
{
    public GameObject rotation;
    void Start()
    {
        Vector3 pos = rotation.transform.position;
        pos.z = CharacterController.instance.transform.position.z;
        rotation.transform.position = pos;
    }
}
