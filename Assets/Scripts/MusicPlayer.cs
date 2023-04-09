using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	[SerializeField] private AudioClip menuMusic;
	[SerializeField] private AudioClip gameMusic;

	private AudioSource audioSource;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
		StartMenuMusic();
		//TODO: make into a singleton
	}

	private void Update() {
		audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
	}

	public void StartMenuMusic() {
		if(menuMusic) audioSource.clip = menuMusic;
		audioSource.Play();
	}

	public void StartGameMusic() {
		if(gameMusic) audioSource.clip = gameMusic;
		audioSource.Play();
	}
}