using UnityEngine;

public class StrenghtManager : MonoBehaviour
{
    [Header("Scripts")]
    public JointManager joint;
    public FeetGrounding leftGrounding;
    public FeetGrounding rightGrounding;
    public SpeedTracker tracker;
    public DetectCollision detect;

    [Header("Bools")]
    public bool Waiting = false;
    public bool KnockedOut = false;
    private bool GetUp = false;

    [Header("Strenght")]
    private float strenght = 1f; // the strenghtMultiplier takes values from here without looping its own

    [Header("Grounding")]
    public float leftImpact; // The amount of strenght taken when feet is not grounded
    public float rightImpact;
    public float impactMultiplier = 1f;

    [Header("SpeedDebuff")]
    public float forceMultiplier = 0.01f; // Affects the intensity of knocked out when going fast
    private float Force;
    public float minimalForce = 10f; // The minimum speed for downscaling strenght

    [Header("Knocking")]
    public float delay; // delay that is being applyed to WaitForSeconds
    public float baseDelay = 0.1f;
    public float minimalKnockOutSpeed = 10f;
    public float knockOutStrenght = 0f;
    private float knockOutReboot = 1f; // A Brother to knockOutStrenght
    public float getUpSpeed = 0.1f;

    private void Update()
    {
        // Reference to the grounded bool in Feets
        bool L = leftGrounding.isGrounded;
        bool R = rightGrounding.isGrounded;

        joint.strenghtMultiplier = strenght - (leftImpact + rightImpact) - Force - knockOutStrenght; // calculate final strenght

        strenght = Mathf.Round(strenght * 100f) / 100f;  // Rounds the strenght value

        // Apply delay and KnockOut if head is hit too hard
        if(detect.collided && !KnockedOut && tracker.Speed > minimalKnockOutSpeed)
        {
            delay = baseDelay * tracker.Speed;
            KnockOut();
        }

        delay = Mathf.Clamp(delay, 0f, 10f);

        // Smoothly Stand Up after being Knocked Out
        knockOutReboot = Mathf.Clamp(knockOutReboot, 0, 1);

        if (GetUp)
        {
            knockOutStrenght = knockOutReboot - getUpSpeed * Time.deltaTime;
        }
        else
        {
            knockOutReboot = 0f;
        }

        if(KnockedOut && !Waiting)
        {
            Concious();
        }

        if(tracker.Speed > minimalForce)
        {
            Force = tracker.Speed * forceMultiplier;
        }
        else
        {
            Force = 0f;
        }

        if (L)
        {
            leftImpact = 0f;
        }
        else if (!L)
        {
            leftImpact = 0.25f * impactMultiplier;
        }

        if (R)
        {
            rightImpact = 0f;
        }
        else if (!R)
        {
            rightImpact = 0.25f * impactMultiplier;
        }
    }

    public void KnockOut()
    {
        knockOutStrenght = 1f;
        knockOutReboot = 1f;
        GetUp = false;

        if (!Waiting && !KnockedOut)
            StartCoroutine(OutAndConcious(true));
    }

    public void Concious()
    {
        delay = 0f;
        GetUp = true;

        if (!Waiting && KnockedOut)
            StartCoroutine(OutAndConcious(false));
    }

    private System.Collections.IEnumerator OutAndConcious(bool State)
    {
        Waiting = true;
        KnockedOut = State;
        yield return new WaitForSeconds(delay);
        Waiting = false;
    }

}
