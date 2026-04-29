using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public PhysicsButton respawnButton;
    public float respawnDelay = 1.5f;
    public DeathCount counter;
    public float cooldown = 2f;
    private bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (onCooldown) return;
        HealthManager health = other.GetComponentInParent<HealthManager>();
        if (health != null)
        {
            onCooldown = true;
            health.TriggerDeath();
            StartCoroutine(Respawn());
            counter.deathCount++;
        }
    }

    private System.Collections.IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        respawnButton.StartCoroutine(respawnButton.Teleport());
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}