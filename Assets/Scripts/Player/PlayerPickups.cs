using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickups : MonoBehaviour {

	[SerializeField] private GameObject doorLockedPrompt;
	[SerializeField] private GameObject doorUnlockedPrompt;
	[SerializeField] private GameObject lootButtonPrompt;
    [SerializeField] private AudioClip lootPickupSound;

	private LevelChanger levelChanger;
    private PlayerShooting playerShoot;
    private AudioSource audioSource;
    private bool canEnterDoor;
    private bool inLootTrigger;
    private GameObject lootObj;

    private void Awake() {
        playerShoot = GetComponent<PlayerShooting>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        canEnterDoor = false;
        inLootTrigger = false;
        lootObj = null;
    }

    private void Update() {
        //when the interact key pressed...
		if(Input.GetKeyDown(KeyCode.F)) {
            //if next to a door that can be exited through, start a new level and exit the trigger
			if(canEnterDoor) {
				levelChanger.StartNewLevel();
				ExitDoorTrigger();
            //if on top of a pickup, destroy the pickup, add a rocket, and exit the trigger
			} else if(inLootTrigger) {
                Destroy(lootObj);
                playerShoot.AddRocket();
                ExitLootTrigger();
			}
		}
	}

	public void EnterDoorTrigger(LevelChanger door, bool doorUnlocked) {
		if(doorUnlocked) {
			canEnterDoor = true;
			doorUnlockedPrompt.SetActive(true);
		} else {
			canEnterDoor = false;
			doorLockedPrompt.SetActive(true);
		}
		levelChanger = door;
	}

	public void ExitDoorTrigger() {
		canEnterDoor = false;
		levelChanger = null;
		doorLockedPrompt.SetActive(false);
		doorUnlockedPrompt.SetActive(false);
	}

	public void EnterLootTrigger(GameObject loot) {
        inLootTrigger = true;
        if(lootObj == null) { //null check in case the player walks over multiple pickups
            lootObj = loot;
        }
        lootButtonPrompt.SetActive(true);
    }

    public void ExitLootTrigger() {
        inLootTrigger = false;
        lootObj = null;
        lootButtonPrompt.SetActive(false);
    }
}