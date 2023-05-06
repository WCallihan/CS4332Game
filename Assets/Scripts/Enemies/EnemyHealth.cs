using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Hurt Settings")]
    [SerializeField] private GameObject enemyVisuals;
    [SerializeField] private Material defaultEnemyMat;
    [SerializeField] private Material hurtEnemyMat;
    [SerializeField] private AudioClip hurtSound;

    [Header("Loot Spawning")]
    [SerializeField] private GameObject rocketPickupPrefab;
    [SerializeField, Range(0, 1)] private float spawnChance;

    private AudioSource audioSource;
    private int currentHealth;

	public event Action EnemyDied;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage; //damage the enemy
        StartCoroutine(HurtFlash()); //flash the enemy's eyes red in response to damage
        if(hurtSound) audioSource.PlayOneShot(hurtSound); //play hurt sound effect
        if(currentHealth <= 0) {
            Die(); //kill enemy
        }
    }

    private void Die() {
        //try to spawn rocket pickup
        float rand = UnityEngine.Random.Range(0f, 1f);
        if(rand <= spawnChance) Instantiate(rocketPickupPrefab, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);

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