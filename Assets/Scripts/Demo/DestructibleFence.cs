using UnityEngine;

public class DestructibleFence : MonoBehaviour
{
    public GameObject[] phases;
    private int currentPhase = 0;

    public float hitCooldown = 0.5f;
    private float lastHitTime;

    private void Start()
    {
        UpdatePhase();
    }

    public void Hit()
    {
        if (Time.time - lastHitTime < hitCooldown) return;
        lastHitTime = Time.time;

        if (currentPhase >= phases.Length - 1) return;

        currentPhase++;
        UpdatePhase();
    }

    private void UpdatePhase()
    {
        for (int i = 0; i < phases.Length; i++)
            phases[i].SetActive(i == currentPhase);
    }
}