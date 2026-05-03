using UnityEngine;

public class PunchImpact : MonoBehaviour
{
    public Punching punching;

    private void OnCollisionEnter(Collision collision)
    {
        if (!punching.isPunching) return;
        DestructibleFence fence = collision.gameObject.GetComponentInParent<DestructibleFence>();
        if (fence != null)
            fence.Hit();

        CycleBlock block = collision.gameObject.GetComponent<CycleBlock>();
        if (block != null)
            block.Hit();
    }
}