using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage = 25f;
    public Collider damageCollider;

    private void OnCollisionEnter(Collision collision)
    {
        if (damageCollider != null && collision.contacts[0].thisCollider != damageCollider) return;

        HealthManager health = collision.gameObject.GetComponentInParent<HealthManager>();
        if (health != null)
            health.TakeDamage(damage);
    }
}