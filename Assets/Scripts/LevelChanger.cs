using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {
	
    //called by the PlayButton on the MainMenu and by interacting with ending doors in all room levels
    public void StartNewLevel() {
        /*
         * TODO: choose a random new room that you're not currently in out of the list of room scenes
         *          load the new scene and update scoring on the player singleton
         *          move the player singleton to the scene's player starting position
         *          and do other stuff as necessary
         */
        Debug.Log("Starting new level");
    }

    //called by the MenuButton on the PauseScreen
    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}