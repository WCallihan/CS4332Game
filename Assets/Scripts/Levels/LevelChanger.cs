using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    [SerializeField] private AudioClip unlockingSound;

	private EnemyWaveSpawner enemies;
    private AudioSource audioSource;
	private bool doorUnlocked;
    private static bool gameStarted;

    public static event Action MenuStarted;
    public static event Action GameStarted;

	private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if(audioSource) audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        doorUnlocked = false;

        //invoke game events for the music player
        if(SceneManager.GetActiveScene().name == "MainMenu") {
            MenuStarted?.Invoke();
            gameStarted = false;
        } else if(!gameStarted) {
            GameStarted?.Invoke();
            gameStarted = true;
        }
	}

	private void Start() {
		enemies = FindObjectOfType<EnemyWaveSpawner>();
		if(enemies != null) enemies.AllEnemiesDead += UnlockDoor;
	}

	//called by the PlayButton on the MainMenu and by interacting with ending doors in all room levels
	public void StartNewLevel() {
        //invoke game started event for music player
        if(!gameStarted) {
            GameStarted?.Invoke();
            gameStarted = true;
        }

        //find new room to load and make sure sure it's different than the current one
        string newRoom;
        string currentRoom = SceneManager.GetActiveScene().name;
        do {
            int index = UnityEngine.Random.Range(1, 5);
            newRoom = "Room" + index;
        } while(newRoom == currentRoom);

        //TODO: update player score if not coming from the menu

        SceneManager.LoadScene(newRoom);
        Debug.Log("Starting new level");
    }

    void OnTriggerEnter(Collider other) {

        Debug.Log("trigger enter");

		PlayerPickups player = other.GetComponent<PlayerPickups>();

		if(player != null) {
			player.EnterDoorTrigger(this, doorUnlocked);
        }
    }

	private void OnTriggerExit(Collider other) {

		PlayerPickups player = other.GetComponent<PlayerPickups>();

		if(player != null) {
			player.ExitDoorTrigger();
		}
	}

	private void UnlockDoor() {
		doorUnlocked = true;
        if(unlockingSound && audioSource) audioSource.PlayOneShot(unlockingSound);
	}

	//called by the MenuButton on the PauseScreen
	public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}