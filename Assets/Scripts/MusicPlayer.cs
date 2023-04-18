using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	[SerializeField] private AudioClip menuMusic;
	[SerializeField] private AudioClip gameMusic;

	private AudioSource audioSource;

	//singleton instance variables
	private static MusicPlayer instance;
	public static MusicPlayer Instance { get { return instance; } }

    private void OnEnable() {
        //subscribe to events for music change
        LevelChanger.MenuStarted += StartMenuMusic;
        LevelChanger.GameStarted += StartGameMusic;
        PlayerHealth.PlayerDied += StopMusic;
    }

    private void OnDisable() {
        //unsubscribe to events
        LevelChanger.MenuStarted -= StartMenuMusic;
        LevelChanger.GameStarted -= StartGameMusic;
        PlayerHealth.PlayerDied -= StopMusic;
    }

    private void Awake() {
		audioSource = GetComponent<AudioSource>();

		//singleton logic
		DontDestroyOnLoad(gameObject);
        if(instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
	}

	private void Update() {
		audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
	}

	public void StartMenuMusic() {
        Debug.Log("Starting Menu Music");
		if(menuMusic) audioSource.clip = menuMusic;
		audioSource.Play();
	}

	public void StartGameMusic() {
        Debug.Log("Starting Game Music");
		if(gameMusic) audioSource.clip = gameMusic;
		audioSource.Play();
	}

    public void StopMusic() {
        Debug.Log("Ending Music");
        StartCoroutine(FadeOutMusic(1));
    }

    private IEnumerator FadeOutMusic(float fadeTime) {
        //save starting volume
        float startVolume = audioSource.volume;

        //slowly decrease volume over the fade time
        while(audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        //stop audio and reset volume for next time
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}