using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Death Screen")]
	[SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI previousBestText;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    private bool paused;

    public static event Action<bool> GamePaused;

	private void OnEnable() {
		PlayerHealth.PlayerDied += ShowDeath;
	}

	private void OnDisable() {
		PlayerHealth.PlayerDied -= ShowDeath;
	}

	private void Awake() {
        Unpause();
		deathScreen.SetActive(false);
	}

    private void Update() {
        //escape toggles the pause menu
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(paused) {
                Unpause();
            } else {
                Pause();
            }
        }
    }

    //called by pause input
    private void Pause() {
        //set the pause flag
        paused = true;
        //freeze time
        Time.timeScale = 0;
        //show the pause screen
        pauseScreen.SetActive(true);
        //unlock and show mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //send out event
        GamePaused?.Invoke(true);
    }

    //called by pause input and by the Resume button on PauseScreen
    public void Unpause() {
        //set time back to normal
        Time.timeScale = 1;
        //hide the pause screen
        pauseScreen.SetActive(false);
        //lock and hide mouse during gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //send out event
        GamePaused?.Invoke(false);
        //set the pause flag
        paused = false;
    }

	private void ShowDeath() {
        //set high score text
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        previousBestText.text = "Previous Best: " + highScore;
        //see if the current score is a high score and display appropriately
        int currentScore = FindObjectOfType<PlayerScore>().Score;
        currentScoreText.text = "Rooms Cleared: " + currentScore;
        if(currentScore > highScore) {
            //new high score
            currentScoreText.color = Color.green;
            PlayerPrefs.SetInt("HighScore", currentScore);
        } else {
            //not a new high score
            currentScoreText.color = Color.white;
        }

		//freeze time
		Time.timeScale = 0;
		//show the death screen
		deathScreen.SetActive(true);
		//unlock and show mouse
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		//reuse game paused event since it does the same thing
		GamePaused?.Invoke(true);
	}
}