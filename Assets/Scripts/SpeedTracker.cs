using UnityEngine;

public class SpeedTracker : MonoBehaviour
{
    Vector3 previousPosition;
    Vector3 currentVelocity;

    public float Speed;

    private void Update()
    {
        Vector3 displacement = transform.position - previousPosition;
        currentVelocity = displacement / Time.deltaTime;
        previousPosition = transform.position;

        // Horizontal Velocity (on plane)
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        Speed = horizontalVelocity.magnitude;
    }
}
