using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    private CharacterController characterController;
    
    public AudioClip[] defaultFootstepSounds;
    public AudioClip[] grassFootstepSounds;
    public AudioClip[] metalFootstepSounds;

    private AudioSource audioSource;

    public float footstepCooldown;
    private float nextFootstepTime;

    private bool wasMoving = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            float movementMagnitude = characterController.velocity.magnitude;

            if (movementMagnitude > 0.1f)
            {
                if (!wasMoving && Time.time >= nextFootstepTime)
                {
                    PlayFootstepSound();
                    nextFootstepTime = Time.time + footstepCooldown;
                }
                wasMoving = true;
            }
            else
            {
                wasMoving = false;
            }
        }
    }

    private void PlayFootstepSound()
    {
        string groundTag = GetGroundTag();

        if (groundTag == "Grass")
        {
            PlayRandomFootstepSound(grassFootstepSounds);
        }
        else if (groundTag == "Metal")
        {
            PlayRandomFootstepSound(metalFootstepSounds);
        }
        else
        {
            PlayRandomFootstepSound(defaultFootstepSounds);
        }
    }

    private string GetGroundTag()
    {
        Vector3 moveDirection = characterController.velocity.normalized;
        float distance = characterController.height / 2f + 0.1f;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            return hit.collider.tag;
        }
        else if (Physics.Raycast(transform.position + moveDirection * distance, Vector3.down, out hit, distance))
        {
            return hit.collider.tag;
        }

        return "";
    }

    private void PlayRandomFootstepSound(AudioClip[] footstepSounds)
    {
        if (footstepSounds.Length > 0)
        {
            AudioClip randomFootstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(randomFootstepSound);
        }
    }
}