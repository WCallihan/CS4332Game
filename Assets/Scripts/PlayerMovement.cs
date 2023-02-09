using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float _walkSpeed = 12f;
    [SerializeField] float _sprintSpeed = 25f;
    [SerializeField] Transform _groundCheck;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 3f;
    [SerializeField] AudioClip _jumpSound;

    private CharacterController _characterControler;
    private AudioSource _audioSource;

    private float _speed;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _hasDoubleJumped = false;
    private bool _wasSprintingWhenJumped = false;

    void Awake() {
        _characterControler = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        //check if on ground
        _isGrounded = Physics.CheckSphere(_groundCheck.position, 0.6f, _groundMask);
        if(_isGrounded && _velocity.y < 0) {
            _velocity.y = -2f; //just makes sure the player sticks to the ground
            _hasDoubleJumped = false; //reset double jump when grounded
            _wasSprintingWhenJumped = false; //reset sprinting momentum when grounded
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
        if((Input.GetKey(KeyCode.LeftShift) && _isGrounded) || (!_isGrounded && _wasSprintingWhenJumped)) {
            _speed = _sprintSpeed;
        } else {
            _speed = _walkSpeed;
        }
        _characterControler.Move(moveVector * _speed * Time.deltaTime);

        //jump
        if(Input.GetButtonDown("Jump") && (_isGrounded || !_hasDoubleJumped)) {
            //check if this is the first jump and if the player was sprinting
            if(_isGrounded && _speed == _sprintSpeed) {
                _wasSprintingWhenJumped = true; //used to keep sprinting speed in air
            }
            //jump upwards
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            //check if this was a double jump
            if(!_isGrounded) {
                _hasDoubleJumped = true; //once you've jumped in air, can't jump again
            }
            //play jump sound effect
            if(_jumpSound) _audioSource.PlayOneShot(_jumpSound);
        }

        //simulate gravity
        _velocity.y += _gravity * Time.deltaTime;
        _characterControler.Move(_velocity * Time.deltaTime);
    }
}