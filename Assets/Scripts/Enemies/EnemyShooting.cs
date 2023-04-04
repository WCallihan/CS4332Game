using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    [Header("Raycast Settings")]
    [SerializeField] float weaponRange = 50f;
    [SerializeField] Transform leftRayOrigin;
    [SerializeField] Transform rightRayOrigin;
    [SerializeField] float lineMaxDuration = 0.1f;

    [Header("Weapons Settings")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int weaponDamage = 20;
    [SerializeField, Range(0, 100)] float weaponAccuracy; //100 = perfect accuracy, 0 = never going to hit
    [SerializeField] AudioClip shootingSound;

    private GameObject player;
    private RaycastHit objectHit;
	private Transform currentRayOrigin;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private bool playerSighted = false;
    private float attackTimer;
    private float lineTimer;
    
    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        Mathf.Clamp(weaponAccuracy, 0f, 100f); //clamps accuracy between 0% and 100%
        playerSighted = false;
		currentRayOrigin = leftRayOrigin;
    }

    void Update() {
        //make sure line renderer is enabled on each frame
        lineRenderer.enabled = true;

        //increase attack timer
        attackTimer -= Time.deltaTime;

        //shoot the player if they are sighted and the attack cooldown is up
        if(playerSighted && attackTimer <= 0) {
            attackTimer = attackCooldown; //reset attack timer
            lineTimer = lineMaxDuration; //reset line timer
            Shoot();
        }

        //disable line renderer after max duration to make line disappear
        lineTimer -= Time.deltaTime;
        if(lineTimer <= 0) {
            lineRenderer.enabled = false;
        }
    }

    //used to give the weapon range to EnemyMovement
    public float GetWeaponRange() { return weaponRange; }

    //called by Enemy Movement to trigger the enemy to start shooting
    public void PlayerSighted(GameObject p) {
        playerSighted = true;
        player = p;
    }

    private void Shoot() {
        //calculate direction and probable end point of the bullet
        Vector3 perfectRayDirection = (player.transform.position - currentRayOrigin.position).normalized; //perfect ray shoots directly from the gun to the player
        float missRate = 100 - weaponAccuracy;
        float randX = Random.Range(-missRate, missRate) / 100;
        float randY = Random.Range(-missRate, missRate) / 100;
        float randZ = Random.Range(-missRate, missRate) / 100;
        Vector3 actualRayDirection = perfectRayDirection + new Vector3(randX, randY, randZ); //makes a random direction based on weapon accuracy to make shooting unpredictable
        Vector3 endPosition = currentRayOrigin.position + (actualRayDirection * weaponRange);

        lineRenderer.SetPosition(0, currentRayOrigin.position); //sets begining of visual line

        //shoot raycast
        if(Physics.Raycast(currentRayOrigin.position, actualRayDirection, out objectHit, weaponRange)) {
            lineRenderer.SetPosition(1, objectHit.point); //sets end of visual line if it hits
            //damage the player if they are hit
            PlayerHealth player = objectHit.transform.gameObject.GetComponent<PlayerHealth>();
            player?.TakeDamage(weaponDamage);

        } else {
            lineRenderer.SetPosition(1, endPosition); //sets end of visual line if it misses
        }

        //play the shooting sound effect
        if(shootingSound) audioSource.PlayOneShot(shootingSound);

		//switch the current ray origin
		if(currentRayOrigin == leftRayOrigin) {
			currentRayOrigin = rightRayOrigin;
		} else {
			currentRayOrigin = leftRayOrigin;
		}
	}
}