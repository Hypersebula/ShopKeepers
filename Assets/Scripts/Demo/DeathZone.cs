using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public PhysicsButton respawnButton;
    public float respawnDelay = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        HealthManager health = other.GetComponentInParent<HealthManager>();
        if (health != null)
        {
            health.TriggerDeath();
            StartCoroutine(Respawn());
        }
    }

    private System.Collections.IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        respawnButton.StartCoroutine(respawnButton.Teleport());
    }
}