using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("Main Screen")]
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Options Screen")]
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;

	private void Start() {
        //set high score text
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if(highScore > 0) {
            highScoreText.text = "Most Rooms Cleared: " + highScore;
        } else {
            highScoreText.text = "";
        }

        //set initial slider values
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);

        //create listeners to update the player prefs
		sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
		musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
	}

	private void UpdateSFXVolume(float v) {
		PlayerPrefs.SetFloat("SFXVolume", v);
	}

	private void UpdateMusicVolume(float v) {
		PlayerPrefs.SetFloat("MusicVolume", v);
	}
}