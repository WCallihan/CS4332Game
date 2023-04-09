using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    [SerializeField] private GameObject pauseScreen;

    private bool paused;

	private void Awake() {
        Unpause();
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
        //set the pause flag
        paused = false;
    }
}