using UnityEngine;

public class LookAround : MonoBehaviour
{
    [Header("References")]
    public Transform playerRoot; // After Clamp Rotation
    public Transform headPivot; // Here goes the Head Object
    public Camera mainCamera; // Here goes the Camera

    [Header("Settings")]
    public float sensitivity = 2f; // Sensitivity
    public float pitchClamp = 80f; // Vertical Clamp
    public float yawClamp = 25f; // Horizontal Clamp

    float yawHead = 0; // Horizontal Rotation
    float pitch = 0; // Vertical Rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Capture Mouse Movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Look Up/Down
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // Try Rotating Head Left/Right
        yawHead += mouseX;
        
        // If head turns too far, rotate body and clamp head
        if (yawHead > yawClamp)
        {
            float excess = yawHead - yawClamp;
            yawHead = yawClamp;
            playerRoot.Rotate(0f, excess, 0f);
        }
        else if (yawHead < -yawClamp)
        {
            float excess = yawHead + yawClamp;
            yawHead = -yawClamp;
            playerRoot.Rotate(0f, excess, 0f);
        }

        // Apply local rotation to the head
        headPivot.localRotation = Quaternion.Euler(pitch, yawHead, 0f);
    }
}
