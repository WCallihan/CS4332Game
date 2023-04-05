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

        int index = Random.Range(1, 4);
        SceneManager.LoadScene("Room" + index);
        Debug.Log("Starting new level");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            int index = Random.Range(1, 4);
            SceneManager.LoadScene("Room" + index);
        }
    }

    //called by the MenuButton on the PauseScreen
    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}