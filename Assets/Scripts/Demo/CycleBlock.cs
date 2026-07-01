using UnityEngine;

public class CycleBlock : MonoBehaviour
{
    public Material[] materials;
    public Renderer blockRenderer;
    private int currentIndex = 0;

    public float hitCooldown = 0.5f;
    private float lastHitTime;

    private void Start()
    {
        if (materials.Length > 0)
            blockRenderer.material = materials[currentIndex];
    }

    public void Hit()
    {
        if (Time.time - lastHitTime < hitCooldown) return;
        lastHitTime = Time.time;

        currentIndex = (currentIndex + 1) % materials.Length;
        blockRenderer.material = materials[currentIndex];
    }

    public int GetCurrentIndex() => currentIndex;
}