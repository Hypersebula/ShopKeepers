using UnityEngine;

public class HandContact : MonoBehaviour
{
    public Grabbing grabController;
    public string grabbableTag = "Grabbable";
    public bool active = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!active) return;
        if (collision.gameObject.CompareTag(grabbableTag))
            grabController.OnHandContact(collision.rigidbody, collision.contacts[0].point);

        Debug.Log("Hit: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
    }

}
