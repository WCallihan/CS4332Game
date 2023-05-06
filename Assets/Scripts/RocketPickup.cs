using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPickup : MonoBehaviour {

    //when the player walks over the pickup, send the pickup object as a reference to the player
    void OnTriggerEnter(Collider other) {
        PlayerPickups player = other.GetComponent<PlayerPickups>();
        if(player) player.EnterLootTrigger(gameObject);
    }

    //when the player walks away from the pickup, tell the player
    void OnTriggerExit(Collider other) {
        PlayerPickups player = other.GetComponent<PlayerPickups>();
        if(player) player.ExitLootTrigger();
    }
}