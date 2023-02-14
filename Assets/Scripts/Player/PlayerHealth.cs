using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script is placed on the Player character to control it's health and all UI involved with that

public class PlayerHealth : MonoBehaviour {

    [Header("Health Settings")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;

    [Header("Hurt Settings")]
    [SerializeField] Image flashPanel;
    [SerializeField] float flashLength;
    [SerializeField] AudioClip hurtSound;

    [Header("Death Settings")]
    [SerializeField] GameObject[] playerVisuals;
    [SerializeField] AudioClip deathSound;

    private AudioSource audioSource;
    private int currentHealth;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage; //damage the player
        if(healthBar) healthBar.value = currentHealth; //update health bar
        StartCoroutine(RedFlash()); //flash the screen red
        if(hurtSound) audioSource.PlayOneShot(hurtSound); //play hurt sound effect
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
        //FindObjectOfType<Level01Controller>().DeathScreen(); //call level controller to enter death state
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
}