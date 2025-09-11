using UnityEngine;

public class LookAround : MonoBehaviour
{
    public Transform headBone;
    public Transform bodyBone;

    public float sensitivity = 1f;
    float yawHead = 0; // Horizontal Rotation
    float pitch = 0; // Vertical Rotation

    [SerializeField] public SpeedTracker speedTracker;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float headRotY = headBone.rotation.eulerAngles.y;
        float bodyRotY = bodyBone.rotation.eulerAngles.y;
        float delta = Mathf.DeltaAngle(bodyRotY, headRotY);

        // Capture Mouse Movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Look Up/Down
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -75f, 75f);

        // Try Rotating Head Left/Right
        yawHead += mouseX;

        // Apply local rotation to the head
        headBone.localRotation = Quaternion.Euler(pitch, yawHead, 0f);

        // Move body to head when rotation too far appart
        if(Mathf.Abs(delta) > 35f)
        {
            float turnSpeed = Mathf.Abs(delta) * 4f; // scale speed by delta
            bodyBone.rotation = Quaternion.RotateTowards(
                bodyBone.rotation,
                Quaternion.Euler(0, headRotY, 0),
                turnSpeed * Time.deltaTime
                );
        }

        // Counter Rotation
        Quaternion headRot = transform.rotation;

        // Cancel body Yaw
        Quaternion counterYaw = Quaternion.Euler(0f, -bodyRotY, 0f);

        // Apply back
        transform.rotation = counterYaw * headRot;

        // Define Speed
        float speed = speedTracker.Speed;

        // Apply Rotation to Body if moving
        if (speed > 0.1f)
        {
            // Get head Yaw
            float headYaw = headBone.eulerAngles.y;

            // Target Rotation for Body
            Quaternion targetRotation = Quaternion.Euler(0f, headYaw, 0f);

            // Rotate body to Head smoothly
            bodyBone.rotation = Quaternion.Slerp(
                bodyBone.rotation,
                targetRotation,
                Time.deltaTime * 5f
                );
        }
    }
}
