using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour {

    [Header("Raycast Settings")]
    [SerializeField] Camera _playerCamera;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] float _weaponRange = 50f;
    [SerializeField] float _lineMaxDuration = 0.1f;

    [Header("Weapon Settings")]
    [SerializeField] int _bulletDamage = 1;
    [SerializeField] float _bulletForce = 300f;
    [SerializeField] AudioClip _shootBulletSound;
    [SerializeField] int _rocketDamage = 3;
    [SerializeField] float _rocketForce = 1000f;
    [SerializeField] float _rocketExplosionRadius = 2f;
    [SerializeField] float _rocketCooldown = 3f;
    [SerializeField] GameObject _rocketPrefab;
    [SerializeField] Slider _rocketCooldownSlider;
    [SerializeField] AudioClip _shootRocketSound;

    private RaycastHit _objectHit;
    private LineRenderer _lineRenderer;
    private AudioSource _audioSource;
    private float _lineTimer = 0f;
    private float _rocketTimer;
    private bool _canShoot;

    void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _rocketTimer = _rocketCooldown;
        if(_rocketCooldownSlider) _rocketCooldownSlider.maxValue = _rocketCooldown;
        _canShoot = true;
    }

    void Update() {
        //make sure line renderer is enabled on each frame
        _lineRenderer.enabled = true;

        //count up the rocket timer and update the slider
        if(_rocketTimer < _rocketCooldown) {
            _rocketTimer += Time.deltaTime;
        }
		if(_rocketCooldownSlider) _rocketCooldownSlider.value = _rocketTimer;

        //on left click, fire a bullet when the game is running
        if(Input.GetKeyDown(KeyCode.Mouse0) && _canShoot) {
            _lineTimer = _lineMaxDuration; //reset line timer whenever a bullet it fired
            FireBullet();
        //on right click, fire a rocket if the cooldown is up and the game is running
        } else if(Input.GetKeyDown(KeyCode.Mouse1) && _rocketTimer >= _rocketCooldown && _canShoot) {
            FireRocket();
            _rocketTimer = 0f; //reset rocket timer
        }

        //disable line renderer after max duration to make line disappear
        _lineTimer -= Time.deltaTime;
        if(_lineTimer <= 0) {
            _lineRenderer.enabled = false;
        }
    }

    //used by the level controller to make sure that the player can't shoot when the game isn't running
    public void SetShooting(bool canShoot) {
        _canShoot = canShoot;
    }

    private void FireBullet() {
        //calculate direction and probable end point of the bullet
        Vector3 rayDirection = _playerCamera.transform.forward; //bullet raycast goes in the direction the camera is facing
        Vector3 endPosition = _rayOrigin.position + (rayDirection * _weaponRange); //calculated end point of the raycast

        _lineRenderer.SetPosition(0, _rayOrigin.position); //sets begining of visual line

        //shoot raycast
        if(Physics.Raycast(_rayOrigin.position, rayDirection, out _objectHit, _weaponRange)) {
            _lineRenderer.SetPosition(1, _objectHit.point); //sets end of visual line if it hits

            //damage the enemy if one is hit
            //EnemyHealth enemy = _objectHit.transform.gameObject.GetComponent<EnemyHealth>();
            //enemy?.TakeDamage(_bulletDamage, "bullet");
            //push the enemy back a little bit
            //enemy?.GetComponent<Rigidbody>().AddForceAtPosition(rayDirection.normalized * _bulletForce, _objectHit.point, ForceMode.Impulse);

        } else {
            _lineRenderer.SetPosition(1, endPosition); //sets end of visual line if it misses
        }
        //play shooting sound effect
        if(_shootBulletSound) _audioSource.PlayOneShot(_shootBulletSound);
    }

    private void FireRocket() {
		if(_rocketPrefab == null) return;

        //calculate direction of the rocket
        Vector3 rocketDirection = _playerCamera.transform.forward; //same as bullet raycast direction
        Quaternion rocketRotation = Quaternion.identity; //instantiation doesn't matter
        rocketRotation.eulerAngles = rocketDirection; //spawning direction of the rocket based on look direction
        //spawn the rocket prefab
        GameObject rocket = Instantiate(_rocketPrefab, _rayOrigin.position, Quaternion.LookRotation(rocketDirection));
        //assign rocket values based on player settings
        //rocket.GetComponent<Rocket>().SetRocket(_rocketDamage, _rocketForce, _rocketExplosionRadius, rocketDirection);
        //play shooting sound effect
        if(_shootRocketSound) _audioSource.PlayOneShot(_shootRocketSound);
    }
}