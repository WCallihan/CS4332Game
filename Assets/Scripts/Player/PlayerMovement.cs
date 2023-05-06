using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this script is placed on the Player character to control its movement and jumping

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float walkSpeed = 12f;
    [SerializeField] float sprintSpeed = 25f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] AudioClip jumpSound;

    private CharacterController characterController;
    private AudioSource audioSource;

    private float speed;
    private Vector3 velocity;
    private bool isGrounded;
    private bool hasDoubleJumped = false;
    private bool wasSprintingWhenJumped = false;

    void Awake() {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
	}

    void Update() {
        //check if on ground
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.6f, groundMask);
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f; //just makes sure the player sticks to the ground
            hasDoubleJumped = false; //reset double jump when grounded
            wasSprintingWhenJumped = false; //reset sprinting momentum when grounded
        }

        //take in inputs
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        //use character controller to move
        Vector3 moveVector = (transform.right * inputX) + (transform.forward * inputZ);
        if(moveVector.magnitude > 1) {
            moveVector /= moveVector.magnitude; //normalizes magnitude so the character doesn't move faster when moving diagonally
        }
        //sprint while holding shift on the ground or if you are in air and were sprinting when you jumped
        if((Input.GetKey(KeyCode.LeftShift) && isGrounded) || (!isGrounded && wasSprintingWhenJumped)) {
            speed = sprintSpeed;
        } else {
            speed = walkSpeed;
        }
        characterController.Move(moveVector * speed * Time.deltaTime);

        //jump
        if(Input.GetButtonDown("Jump") && (isGrounded || !hasDoubleJumped)) {
            //check if this is the first jump and if the player was sprinting
            if(isGrounded && speed == sprintSpeed) {
                wasSprintingWhenJumped = true; //used to keep sprinting speed in air
            }
            //jump upwards
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //check if this was a double jump
            if(!isGrounded) {
                hasDoubleJumped = true; //once you've jumped in air, can't jump again
            }
            //play jump sound effect
            if(jumpSound) audioSource.PlayOneShot(jumpSound);
        }

        //simulate gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}