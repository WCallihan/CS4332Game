using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {
	
    //called by the QuitButton on the MainMenu
    public void Quit() {
        //if Unity Editor is being used, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        //if the build is being used, quit the program
        Application.Quit();
    }
}