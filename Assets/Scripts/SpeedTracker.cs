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

        Vector3 velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z);
        Speed = velocity.magnitude;
        Speed = Mathf.Round(Speed * 10f) / 10f;
    }
}
