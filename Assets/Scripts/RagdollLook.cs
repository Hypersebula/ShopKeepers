using UnityEngine;

public class RagdollLook : MonoBehaviour
{
    public ConfigurableJoint hips;

    public GameObject spineTarget;

    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        Quaternion current = hips.targetRotation;
        Vector3 euler = current.eulerAngles;
        euler.y -= mouseX;
        hips.targetRotation = Quaternion.Euler(euler);

        Quaternion spineCurrent = spineTarget.transform.rotation;
        Vector3 spineEuler = spineCurrent.eulerAngles;
        spineEuler.x -= mouseY;
        mouseY = Mathf.Clamp(mouseY, -45f, 45f);
        spineTarget.transform.rotation = Quaternion.Euler(spineEuler);
    }
}
