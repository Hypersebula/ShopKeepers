using UnityEngine;

public class Level1Movement : MonoBehaviour
{
    public GameObject Ragdoll;
    public Transform TargetLocation;

    public void Teleport()
    {
        Ragdoll.transform.position = TargetLocation.position;
    }
}
