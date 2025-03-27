using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement single; 

    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;

    public Quest quest;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Footstep Settings")]
    public AudioSource footstepSource;
    public List<AudioClip> footstepClips; 
    public float footstepInterval = 0.5f;

    private float footstepTimer = 0f;

    Vector3 velocity;
    bool isGrounded;

    float x = 0;
    float z = 0;

    private void Awake()
    {
        single = this; 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateMove();
    
    }

    void UpdateInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleFootsteps()
    {
        bool isMoving = (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f);
        if (isGrounded && isMoving)
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

        // Pick a random footstep sound from the list
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Count)];
        footstepSource.PlayOneShot(clip);
    }
}
