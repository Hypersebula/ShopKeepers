using UnityEngine;
using System.Collections.Generic;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> walkFootstepClips;
    public List<AudioClip> sprintFootstepClips;

    // Jump and Land Sound Effects
    public AudioClip jumpSound;
    public AudioClip landSound;

    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.3f;
    public float crouchStepInterval = 0.8f;

    public float walkVolume = 0.6f;
    public float sprintVolume = 1f;
    public float crouchVolume = 0.3f;

    public CharacterController controller;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    private float stepTimer = 0f;
    private bool isMoving;

    // Jump and Land State Tracking
    private bool wasGroundedLastFrame = true; // Tracks the previous frame's grounded state
    private bool hasPlayedJumpSound = false; // To ensure jump sound is played only once
    private bool hasPlayedLandSound = false; // To ensure land sound is played only once

    private void Update()
    {
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool isGrounded = controller.isGrounded;

        // Jump sound trigger
        if (!isGrounded && wasGroundedLastFrame && !hasPlayedJumpSound)
        {
            PlayJumpSound();
            hasPlayedJumpSound = true;
        }

        // Land sound trigger
        if (isGrounded && !wasGroundedLastFrame && !hasPlayedLandSound)
        {
            PlayLandSound();
            hasPlayedLandSound = true;
        }

        // Play footsteps
        if (isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = GetStepInterval();
            }
        }
        else
        {
            stepTimer = 0f;
        }

        // Reset flags *after* processing state changes
        if (!isGrounded)
        {
            hasPlayedLandSound = false;
        }
        else
        {
            hasPlayedJumpSound = false;
        }

        wasGroundedLastFrame = isGrounded;
    }


    private void PlayFootstep()
    {
        if (!controller.isGrounded) return;

        audioSource.volume = GetStepVolume();

        // Play the appropriate footstep sound based on whether the player is sprinting or walking
        if (Input.GetKey(sprintKey) && sprintFootstepClips.Count > 0)  // Sprinting
        {
            audioSource.PlayOneShot(sprintFootstepClips[Random.Range(0, sprintFootstepClips.Count)]);
        }
        else if (walkFootstepClips.Count > 0)  // Walking
        {
            audioSource.PlayOneShot(walkFootstepClips[Random.Range(0, walkFootstepClips.Count)]);
        }
    }

    private void PlayJumpSound()
    {
        // Play the jump sound when the player jumps
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void PlayLandSound()
    {
        // Play the land sound when the player lands
        if (landSound != null)
        {
            audioSource.PlayOneShot(landSound);
        }
    }

    private float GetStepInterval()
    {
        // Set the interval based on the player's movement state
        if (Input.GetKey(crouchKey)) return crouchStepInterval;
        if (Input.GetKey(sprintKey)) return sprintStepInterval;
        return walkStepInterval;
    }

    private float GetStepVolume()
    {
        // Set the volume based on the player's movement state
        if (Input.GetKey(crouchKey)) return crouchVolume;
        if (Input.GetKey(sprintKey)) return sprintVolume;
        return walkVolume;
    }
}
