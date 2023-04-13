using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickups : MonoBehaviour {

	[SerializeField] private GameObject doorLockedPrompt;
	[SerializeField] private GameObject doorUnlockedPrompt;
	[SerializeField] private GameObject lootButtonPrompt;
    [SerializeField] private AudioClip lootPickupSound;

	private LevelChanger levelChanger;
    private AudioSource audioSource;
    private bool canEnterDoor;
    private bool inLootTrigger;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
}

    private void Update() {
		if(Input.GetKeyDown(KeyCode.F)) {
			if(canEnterDoor) {
				levelChanger.StartNewLevel();
				ExitDoorTrigger();
			} else if(inLootTrigger) {
				//TODO: call the loot's pickup function
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

	//TODO: deal with going in and out of loot triggers
}