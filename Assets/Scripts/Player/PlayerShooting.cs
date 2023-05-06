using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//this script is placed on the Player character to control it's weapons

public class PlayerShooting : MonoBehaviour {

    [Header("Raycast Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float weaponRange = 50f;
    [SerializeField] private float lineMaxDuration = 0.1f;
    [SerializeField] private ParticleSystem bulletHitSparks;

    [Header("Weapon Settings")]
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private AudioClip shootBulletSound;
    
    [SerializeField] private int rocketDamage = 3;
    [SerializeField] private float rocketExplosionRadius = 2f;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform rocketSpawnPos;
    [SerializeField] private AudioClip shootRocketSound;
    [SerializeField] private GameObject rocketArt;
    [SerializeField] private int initRockets = 0;
    [SerializeField] private TextMeshProUGUI rocketCountText;

    private RaycastHit objectHit;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private float lineTimer = 0f;
    private bool canShoot;
    private int numRockets;

    private void OnEnable() {
        UIManager.GamePaused += SetShooting;
    }

    private void OnDisable() {
        UIManager.GamePaused -= SetShooting;
    }

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        canShoot = true;
        numRockets = initRockets;
		if(numRockets > 0) rocketArt.SetActive(true);
		else rocketArt.SetActive(false);
    }

    void Update() {
        //make sure line renderer is enabled on each frame
        lineRenderer.enabled = true;

        //on left click, fire a bullet when the game is running
        if(Input.GetKeyDown(KeyCode.Mouse0) && canShoot) {
            lineTimer = lineMaxDuration; //reset line timer whenever a bullet it fired
            FireBullet();
        }
        //on right click, fire a rocket if you have any
        if(Input.GetKeyDown(KeyCode.Mouse1) && numRockets > 0) {
            FireRocket();
        }

        //disable line renderer after max duration to make line disappear
        lineTimer -= Time.deltaTime;
        if(lineTimer <= 0) {
            lineRenderer.enabled = false;
        }
    }

    //used by the level controller to make sure that the player can't shoot when the game isn't running
    public void SetShooting(bool gamePaused) {
        canShoot = !gamePaused;
    }

    private void FireBullet() {
        //calculate direction and probable end point of the bullet
        Vector3 rayDirection = playerCamera.transform.forward; //bullet raycast goes in the direction the camera is facing
        Vector3 endPosition = rayOrigin.position + (rayDirection * weaponRange); //calculated end point of the raycast

        lineRenderer.SetPosition(0, rayOrigin.position); //sets begining of visual line

        //shoot raycast
        if(Physics.Raycast(playerCamera.transform.position, rayDirection, out objectHit, weaponRange)) {
            lineRenderer.SetPosition(1, objectHit.point); //sets end of visual line if it hits
            var sparks = Instantiate(bulletHitSparks, objectHit.point, bulletHitSparks.transform.rotation); //spawn bullet sparks at hit location
            sparks.transform.LookAt(transform.position); //make the bullet sparks look at the player

            //damage the enemy if one is hit
            EnemyHealth enemy = objectHit.transform.gameObject.GetComponent<EnemyHealth>();
            enemy?.TakeDamage(bulletDamage);
        } else {
            lineRenderer.SetPosition(1, endPosition); //sets end of visual line if it misses
        }
        //play shooting sound effect
        if(shootBulletSound) audioSource.PlayOneShot(shootBulletSound);
    }

    public void AddRocket() {
        numRockets++;
        UpdateRocketCount();
        rocketArt.SetActive(true);
    }

    //old function that was used to fire a rocket that went straight
    private void FireRocket() {
		if(rocketPrefab == null) return;

        //calculate direction of the rocket
        Vector3 rocketDirection = playerCamera.transform.forward; //same as bullet raycast direction
        Quaternion rocketRotation = Quaternion.identity; //instantiation doesn't matter
        rocketRotation.eulerAngles = rocketDirection; //spawning direction of the rocket based on look direction
        //spawn the rocket prefab
        GameObject rocket = Instantiate(rocketPrefab, rocketSpawnPos.position, Quaternion.LookRotation(rocketDirection));
        //assign rocket values based on player settings
        rocket.GetComponent<Rocket>().SetRocket(rocketDamage, rocketExplosionRadius, rocketDirection);
        //play shooting sound effect
        if(shootRocketSound) audioSource.PlayOneShot(shootRocketSound);

        //remove a rocket
        numRockets--;
        UpdateRocketCount();
        if(numRockets <= 0) rocketArt.SetActive(false);
    }

    private void UpdateRocketCount() {
        if(numRockets < 10) {
            rocketCountText.text = "0" + numRockets;
        } else {
            rocketCountText.text = "" + numRockets;
        }
    }
}