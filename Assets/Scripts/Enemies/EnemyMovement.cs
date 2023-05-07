﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour {

    [SerializeField] private float turnDampening; //bigger = faster

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

    void Update() {
        //player is sighted if they come within weapon range
        if(!playerSighted && Vector3.Distance(transform.position, player.transform.position) <= weaponRange) {
            playerSighted = true;
            enemyShooting.PlayerSighted(player); //tells the shooter to start shooting
        }

        //if the player has been seen, move towards them
        if(playerSighted) {
            //set the nav mesh agent destination to the player
            navAgent.destination = player.transform.position;
            //turn over time to face the player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime * turnDampening);
        }
    }
}