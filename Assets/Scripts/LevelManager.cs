using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	private void Awake() {
		//lock and hide mouse during gameplay
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}