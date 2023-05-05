using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] int maxHealth = 5;
    [SerializeField] GameObject enemyVisuals;
    [SerializeField] Material defaultEnemyMat;
    [SerializeField] Material hurtEnemyMat;
    [SerializeField] AudioClip hurtSound;
    private int currentHealth;

    private AudioSource audioSource;

	public event Action EnemyDied;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, string killedBy) {
        currentHealth -= damage; //damage the enemy
        StartCoroutine(HurtFlash()); //flash the enemy's eyes red in response to damage
        if(hurtSound) audioSource.PlayOneShot(hurtSound); //play hurt sound effect
        if(currentHealth <= 0) {
            Die(killedBy); //kill enemy
        }
    }

    private void Die(string killedBy) {
        //increase the score based on what it was killed by
        if(killedBy.Equals("bullet") || killedBy.Equals("hazard")) {
            //_levelController.IncreaseScore(5);
        } else if(killedBy.Equals("rocket")) {
            //_levelController.IncreaseScore(10);
        } else {
            Debug.Log("Bad killedBy variable: " + killedBy);
        }
        gameObject.SetActive(false); //deactivate game object
		EnemyDied?.Invoke();
    }

	
    private IEnumerator HurtFlash() {
        Renderer enemyRenderer = enemyVisuals.GetComponent<Renderer>();
        enemyRenderer.material = hurtEnemyMat; //set the enemy material to the hurt version to set eyes to red
        yield return new WaitForSeconds(0.15f); //wait
        enemyRenderer.material = defaultEnemyMat; //set back to original material
    }
}