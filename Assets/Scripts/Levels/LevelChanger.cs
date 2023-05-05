using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    [SerializeField] private AudioClip unlockingSound;
	[SerializeField] private Animator levelFadeAnimator;

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

    void OnTriggerEnter(Collider other) {
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
        if (currentRoom != "MainMenu") { PlayerScore.instance.AddPoint(); }
        Debug.Log("Added a point");

		StartCoroutine(StartNewLevelDelay(newRoom));
	}

	private IEnumerator StartNewLevelDelay(string newLevel) {
		//start fade out animation; fade in animation is played automatically when a level is loaded
		levelFadeAnimator.SetTrigger("FadeOut");
		//delay for just long enough to fade to black
		yield return new WaitForSeconds(0.5f);
		//load new level; all other logic is handled in various OnAwake functions
		SceneManager.LoadScene(newLevel);
		Debug.Log("Starting " + newLevel);
	}

	private void UnlockDoor() {
		doorUnlocked = true;
        if(unlockingSound && audioSource) audioSource.PlayOneShot(unlockingSound);
	}

	//called by the MenuButton on the PauseScreen
	public void GoToMainMenu() {
        Time.timeScale = 1; //reset time scale
        StartCoroutine(StartNewLevelDelay("MainMenu"));
    }
}