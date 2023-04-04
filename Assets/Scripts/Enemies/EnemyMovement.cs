using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] float speed = 15f;
    //[SerializeField] Vector3 patrolLeftPos, patrolRightPos;

	private GameObject player;
    private EnemyShooting enemyShooting;
    private Rigidbody enemyRb;
    //public Vector3 currentWaypoint;
    public Vector3 moveVector;
    private float weaponRange;
    public bool playerSighted = false;

    void Awake() {
		player = FindObjectOfType<PlayerMovement>().gameObject;
        enemyShooting = GetComponent<EnemyShooting>();
        weaponRange = enemyShooting.GetWeaponRange(); //used to see if player is within sight range
        enemyRb = GetComponent<Rigidbody>();
        playerSighted = false;
        //currentWaypoint = patrolLeftPos; //starts by patrolling to the left
    }

    void FixedUpdate() {
        //player is sighted if they come within weapon range
        if(!playerSighted && Vector3.Distance(transform.position, player.transform.position) <= weaponRange) {
            playerSighted = true;
            enemyShooting.PlayerSighted(player); //tells the shooter to start shooting
        }

        //if the enemy has not seen the player, move towards the current patrol waypoint
        //if(!playerSighted) {
            //moveVector = new Vector3(currentWaypoint.x - transform.position.x, 0, currentWaypoint.z - transform.position.z); //move to waypoint

        //if the player has been seen and is not within a reasonable range, move towards them
        /*} else*/ if(playerSighted && Mathf.Abs(transform.position.x - player.transform.position.x) > 5f &&
                    Mathf.Abs(transform.position.z - player.transform.position.z) > 5f) {
            moveVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z); //move to player

        //if the player is within a reasonable range, stop moving
        } else {
            moveVector = Vector3.zero; //stop moving
        }

        //face the player if they are sighted, or face the move vector if it isn't 0
        if(playerSighted) {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position); //look at the player
        } else if(moveVector.magnitude != 0) {
            transform.rotation = Quaternion.LookRotation(moveVector); //face forward
        }

		/*
        //set next waypoint if one is reached while patroling
        if(Mathf.Abs(moveVector.x) <= 0.1f && Mathf.Abs(moveVector.z) <= 0.1f && !playerSighted) {
            moveVector = Vector3.zero;
            if(currentWaypoint == patrolLeftPos) {
                currentWaypoint = patrolRightPos;
            } else {
                currentWaypoint = patrolLeftPos;
            }
        }
		*/

        //move along the move vector unless too close to the player
        enemyRb.velocity = moveVector.normalized * speed;
    }
}