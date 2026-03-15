using UnityEngine;

public class HandContact : MonoBehaviour
{
    public Grabbing grabController;
    public string grabbableTag = "Grabbable";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(grabbableTag))
            grabController.OnHandContact(collision.rigidbody, collision.contacts[0].point);
    }

}
