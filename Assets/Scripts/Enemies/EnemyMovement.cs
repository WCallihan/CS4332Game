using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private float stoppingDistance; //stopping distance from the player

	private GameObject player;
    private EnemyShooting enemyShooting;
    private NavMeshAgent navAgent;
    private float weaponRange;
    private bool playerSighted;

    void Awake() {
		player = FindObjectOfType<PlayerMovement>().gameObject;
        enemyShooting = GetComponent<EnemyShooting>();
        weaponRange = enemyShooting.GetWeaponRange(); //used to see if player is within sight range
        navAgent = GetComponent<NavMeshAgent>();
        playerSighted = false;
    }

    void FixedUpdate() {
        //player is sighted if they come within weapon range
        if(!playerSighted && Vector3.Distance(transform.position, player.transform.position) <= weaponRange) {
            playerSighted = true;
            enemyShooting.PlayerSighted(player); //tells the shooter to start shooting
        }

        //if the player has been seen and is not within a reasonable range, move towards them
        if(playerSighted && Vector3.Distance(transform.position, player.transform.position) > stoppingDistance) {
            navAgent.destination = player.transform.position; //set the nav mesh agent destination to the player
            navAgent.isStopped = false; //start moving again

        //if the player is within a reasonable range, stop moving
        } else {
            navAgent.isStopped = true; //stop moving
        }

        //face the player if they are sighted and the enemy isn't moving
        if(playerSighted && navAgent.isStopped) {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position); //look at the player
        }
    }
}