using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement single;

    [Header("Movement Settings")]
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;

    public Quest quest;

    [Header("Footstep Settings")]
    public AudioSource footstepSource;
    public List<AudioClip> footstepClips;
    public float footstepInterval = 0.5f;

    private float footstepTimer = 0f;
    private Vector3 velocity;

    private float x = 0f;
    private float z = 0f;

    private void Awake()
    {
        single = this;
    }

    void Update()
    {
        UpdateInput();
        UpdateMove();
        HandleFootsteps();
    }

    void UpdateInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void UpdateMove()
    {
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleFootsteps()
    {
        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (isMoving)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (footstepSource == null || footstepClips == null || footstepClips.Count == 0)
            return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Count)];

        footstepSource.pitch = Random.Range(0.9f, 1.1f);

        footstepSource.PlayOneShot(clip);
    }

}
