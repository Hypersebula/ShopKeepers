using UnityEngine;

public class CamerPositionSwitch : MonoBehaviour
{
    public GameObject frontCam;
    public GameObject idleCam;
    public GameObject backCam;

    public ForwardStateMachine state;

    Vector3 targetOffset;

    public Transform cameraTransform;
    public float positionSmoothTime = 0.08f;

    Vector3 positionVelocity;

    private void FixedUpdate()
    {
        if (state.movingForward)
        {
            targetOffset = frontCam.transform.position;
        }
        else
        {
            targetOffset = idleCam.transform.position;
        }

        if (state.movingBackward)
        {
            targetOffset = backCam.transform.position;
        }
        else
        {
            targetOffset = idleCam.transform.position;
        }

        if (state.idle)
        {
            targetOffset = idleCam.transform.position;
        }
    }

    private void LateUpdate()
    {
        Follow(targetOffset);
    }

    public void Follow(Vector3 offsetTargetWorldPos)
    {
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, offsetTargetWorldPos, ref positionVelocity, positionSmoothTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetOffset, 0.05f);
    }
}
