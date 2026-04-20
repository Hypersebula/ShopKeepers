using UnityEngine;

public class GrabJointBreakListener : MonoBehaviour
{

    public Grabbing grabbing;

    private void OnJointBreak(float breakForce)
    {
        grabbing.OnGrabBroken();
    }
}
