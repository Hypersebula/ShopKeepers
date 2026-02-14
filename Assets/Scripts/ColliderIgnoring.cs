using UnityEngine;

public class ColliderIgnoring : MonoBehaviour
{
    public Collider[] ballIgnore;
    public Collider stabilizedCollider;

    private void Start()
    {
        foreach (Collider ignored in ballIgnore)
        {
            Physics.IgnoreCollision(ignored, stabilizedCollider, true);
        }
    }
}
