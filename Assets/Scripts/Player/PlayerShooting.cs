using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script is placed on the Player character to control it's weapons

public class PlayerShooting : MonoBehaviour {

    [Header("Raycast Settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform rayOrigin;
    [SerializeField] float weaponRange = 50f;
    [SerializeField] float lineMaxDuration = 0.1f;

    [Header("Weapon Settings")]
    [SerializeField] int bulletDamage = 1;
    [SerializeField] AudioClip shootBulletSound;
    /*
    [SerializeField] int grenadeDamage = 3;
    [SerializeField] float grenadeForce = 1000f;
    [SerializeField] float grenadeExplosionRadius = 2f;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] AudioClip throwGrenadeSound;
    */

    private RaycastHit objectHit;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private float lineTimer = 0f;
    private bool canShoot;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
    }

    void Update() {
        //make sure line renderer is enabled on each frame
        lineRenderer.enabled = true;

        //on left click, fire a bullet when the game is running
        if(Input.GetKeyDown(KeyCode.Mouse0) && canShoot) {
            lineTimer = lineMaxDuration; //reset line timer whenever a bullet it fired
            FireBullet();
        }

        //disable line renderer after max duration to make line disappear
        lineTimer -= Time.deltaTime;
        if(lineTimer <= 0) {
            lineRenderer.enabled = false;
        }
    }

    //used by the level controller to make sure that the player can't shoot when the game isn't running
    public void SetShooting(bool canShoot) {
        this.canShoot = canShoot;
    }

    private void FireBullet() {
        //calculate direction and probable end point of the bullet
        Vector3 rayDirection = playerCamera.transform.forward; //bullet raycast goes in the direction the camera is facing
        Vector3 endPosition = rayOrigin.position + (rayDirection * weaponRange); //calculated end point of the raycast

        lineRenderer.SetPosition(0, rayOrigin.position); //sets begining of visual line

        //shoot raycast
        if(Physics.Raycast(rayOrigin.position, rayDirection, out objectHit, weaponRange)) {
            lineRenderer.SetPosition(1, objectHit.point); //sets end of visual line if it hits

            //damage the enemy if one is hit
            EnemyHealth enemy = objectHit.transform.gameObject.GetComponent<EnemyHealth>();
            enemy?.TakeDamage(bulletDamage, "bullet");
        } else {
            lineRenderer.SetPosition(1, endPosition); //sets end of visual line if it misses
        }
        //play shooting sound effect
        if(shootBulletSound) audioSource.PlayOneShot(shootBulletSound);
    }

    //old function that was used to fire a rocket that went straight
    //TODO: change later to throw a grenade prefab
    /*
    private void FireRocket() {
		if(grenadePrefab == null) return;

        //calculate direction of the rocket
        Vector3 rocketDirection = playerCamera.transform.forward; //same as bullet raycast direction
        Quaternion rocketRotation = Quaternion.identity; //instantiation doesn't matter
        rocketRotation.eulerAngles = rocketDirection; //spawning direction of the rocket based on look direction
        //spawn the rocket prefab
        GameObject rocket = Instantiate(grenadePrefab, rayOrigin.position, Quaternion.LookRotation(rocketDirection));
        //assign rocket values based on player settings
        //rocket.GetComponent<Rocket>().SetRocket(_rocketDamage, _rocketForce, _rocketExplosionRadius, rocketDirection);
        //play shooting sound effect
        if(throwGrenadeSound) audioSource.PlayOneShot(throwGrenadeSound);
    }
    */
}