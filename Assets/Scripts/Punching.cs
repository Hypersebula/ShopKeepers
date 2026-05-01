using UnityEngine;

public class Punching : MonoBehaviour
{
    [Header("References")]
    public Transform shoulder;
    public Camera cam;
    public Rigidbody handRigidbody;

    [Header("IK")]
    public Transform ikTarget;
    public Transform ikHome;
    public Transform punchTarget;

    [Header("Settings")]
    public KeyCode punchKey = KeyCode.Mouse0;
    public float punchSpeed = 25f;
    public float returnSpeed = 5f;
    public float punchForce = 500f;
    public float maxPunchDistance = 2f;
    public LayerMask punchMask;
    public float pressTime;
    public float punchRadius = 0.3f;

    [Header("State")]
    public bool isPunching = false;

    private enum PunchState { Idle, Punching, Returning }
    private PunchState state = PunchState.Idle;
    private Vector3 punchGoal;

    private void Update()
    {
        if (Input.GetKeyDown(punchKey))
            pressTime = Time.time;

        if (Input.GetKeyUp(punchKey) && Time.time - pressTime < 0.25f)
            StartPunch();

        UpdateIK();
    }

    private void StartPunch()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.SphereCast(ray, punchRadius, out RaycastHit hit, maxPunchDistance, punchMask))
            punchGoal = hit.point;
        else
            punchGoal = cam.transform.position + cam.transform.forward * maxPunchDistance;

        punchTarget.position = punchGoal;
        state = PunchState.Punching;
        isPunching = true;
    }

    private void UpdateIK()
    {
        switch (state)
        {
            case PunchState.Punching:
                ikTarget.position = Vector3.Lerp(ikTarget.position, punchTarget.position, Time.deltaTime * punchSpeed);
                if (Vector3.Distance(ikTarget.position, punchTarget.position) < 0.1f)
                {
                    ApplyForce();
                    state = PunchState.Returning;
                }
                break;

            case PunchState.Returning:
                ikTarget.position = Vector3.Lerp(ikTarget.position, ikHome.position, Time.deltaTime * returnSpeed);
                if (Vector3.Distance(ikTarget.position, ikHome.position) < 0.05f)
                {
                    state = PunchState.Idle;
                    isPunching = false;
                }
                break;
        }
    }

    private void ApplyForce()
    {
        handRigidbody.AddForce(cam.transform.forward * punchForce, ForceMode.Impulse);
    }
}