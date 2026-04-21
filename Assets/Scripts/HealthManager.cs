using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("References")]
    public RagdollStateController ragdollStateController;

    [Header("Death")]
    public bool isDead = false;
    public float deathFallDuration = 0.5f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f)
        {
            TriggerDeath();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    [ContextMenu("TriggerDeath")]
    public void TriggerDeath()
    {
        if (isDead) return;
        isDead = true;
        StartCoroutine(ragdollStateController.LerpMultiplier(0f, deathFallDuration));
    }
}
