using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script is placed on the Player character to control it's health and all UI involved with that

public class PlayerHealth : MonoBehaviour {

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;
	[SerializeField] private Image healthFill;
	[SerializeField] private Color normalHealthColor;
	[SerializeField] private Color criticalHealthColor;

    [Header("Hurt Settings")]
    [SerializeField] private Image flashPanel;
    [SerializeField] private float flashLength;
	[SerializeField] private Transform playerCamera;
	[SerializeField] private AnimationCurve cameraShakeCurve;
	[SerializeField] private float cameraShakeDuration;
    [SerializeField] private AudioClip hurtSound;

    [Header("Death Settings")]
    [SerializeField] private GameObject[] playerVisuals;
    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;
    private int currentHealth;
	private bool healthCritical;

    public static event Action PlayerDied;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        currentHealth = maxHealth;
		healthBar.maxValue = maxHealth;
		healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage; //damage the player
        if(healthBar) healthBar.value = currentHealth; //update health bar
        StartCoroutine(RedFlash()); //flash the screen red
		StartCoroutine(CameraShake()); //shake the camera
        if(hurtSound) audioSource.PlayOneShot(hurtSound); //play hurt sound effect

		SetHealthBarColor();
        if(currentHealth <= 0) {
            Die(); //kill player
        }
    }

    public bool Heal(int healing) {
        //if the player is not at full health, heal them and update the health bar, and return true
        if(currentHealth < maxHealth) {
            currentHealth += healing; //heal the player
            if(currentHealth > maxHealth) {
                currentHealth = maxHealth; //make sure the health doesn't exceed the max
            }

            if(healthBar) healthBar.value = currentHealth; //update health bar
			SetHealthBarColor();

            return true;
        //if the player is at full health, do nothing and return false
        } else {
            return false;
        }
    }

    private void Die() {
        Time.timeScale = 0f; //freeze the game
        //deactivate all player visuals
        foreach(var v in playerVisuals) {
            v.SetActive(false);
        }
        if(deathSound) audioSource.PlayOneShot(deathSound); //play death sound effect
        PlayerDied?.Invoke(); //call player death event for music player
        //TODO: show death screen
    }

	private void SetHealthBarColor() {
		if(!healthCritical && (currentHealth <= maxHealth / 3)) {
			healthCritical = true;
			healthFill.color = criticalHealthColor;
		} else if(healthCritical && (currentHealth > maxHealth / 3)) {
			healthCritical = false;
			healthFill.color = normalHealthColor;
		}
	}

    private IEnumerator RedFlash() {
		if(flashPanel == null) yield return null;

        float fadeLength = flashLength / 2;
        //fade red in
        for(float i = 0; i < fadeLength; i += Time.deltaTime) {
            Color panelColor = flashPanel.color;
            panelColor.a = Mathf.Lerp(0, 0.25f, i/fadeLength);
            flashPanel.color = panelColor;
            yield return null;
        }
        
        //fade red out
        for(float i = 0; i < fadeLength; i += Time.deltaTime) {
            Color panelColor = flashPanel.color;
            panelColor.a = Mathf.Lerp(0.25f, 0, i/fadeLength);
            flashPanel.color = panelColor;
            yield return null;
        }

        //ensure alpha is back at 0
        flashPanel.color = new Color(255, 0, 0, 0);
    }

	private IEnumerator CameraShake() {
		//save original position
		Vector3 startPosition = playerCamera.transform.localPosition;

		//shake camera for duration
		float timer = 0;
		while(timer < cameraShakeDuration) {
			//get the strength of the shake from the curve
			float strength = cameraShakeCurve.Evaluate(timer / cameraShakeDuration);
			playerCamera.transform.localPosition = startPosition + UnityEngine.Random.insideUnitSphere * strength;
			timer += Time.deltaTime;
			yield return null;
		}

		//set back to original position
		playerCamera.transform.localPosition = startPosition;
	}
}