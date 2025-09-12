using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool collided = false;
    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collided = false;
    }
}
