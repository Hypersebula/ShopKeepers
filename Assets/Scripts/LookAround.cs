using UnityEngine;

public class LookAround : MonoBehaviour
{
    public Transform headBone;
    public Transform bodyBone;

    public float sensitivity = 1f;
    float yawHead = 0; // Horizontal Rotation
    float pitch = 0; // Vertical Rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Capture Mouse Movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Look Up/Down
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // Try Rotating Head Left/Right
        yawHead += mouseX;

        // Apply local rotation to the head
        headBone.localRotation = Quaternion.Euler(0f, yawHead, 0f);

        // Apply vertical rotation to spine
        bodyBone.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
