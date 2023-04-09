using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;

	private void Start() {
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);

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