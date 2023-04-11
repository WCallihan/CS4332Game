using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	private EnemyWaveSpawner enemies;
	private bool doorUnlocked;

	private void Awake() {
		doorUnlocked = false;
	}

	private void Start() {
		enemies = FindObjectOfType<EnemyWaveSpawner>();
		if(enemies != null) enemies.AllEnemiesDead += UnlockDoor;
	}

	//called by the PlayButton on the MainMenu and by interacting with ending doors in all room levels
	public void StartNewLevel() {
        /*
         * TODO: choose a random new room that you're not currently in out of the list of room scenes
         *          load the new scene and update scoring on the player singleton
         *          move the player singleton to the scene's player starting position
         *          and do other stuff as necessary
         */

        int index = Random.Range(1, 5);
        SceneManager.LoadScene("Room" + index);
        Debug.Log("Starting new level");
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

	private void UnlockDoor() {
		doorUnlocked = true;
	}

	//called by the MenuButton on the PauseScreen
	public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}