using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] int maxHealth = 5;
    [SerializeField] GameObject enemyVisuals;
    [SerializeField] AudioClip hurtSound;
    private int currentHealth;

    private AudioSource audioSource;
    //private Level01Controller _levelController;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        //_levelController = FindObjectOfType<Level01Controller>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, string killedBy) {
        currentHealth -= damage; //damage the enemy
        StartCoroutine(RedFlash()); //flash the enemy red
        if(hurtSound) audioSource.PlayOneShot(hurtSound); //play hurt sound effect
        if(currentHealth <= 0) {
            //Die(killedBy); //kill enemy
        }
    }

	/*
    private void Die(string killedBy) {
        //increase the score based on what it was killed by
        if(killedBy.Equals("bullet") || killedBy.Equals("hazard")) {
            _levelController.IncreaseScore(5);
        } else if(killedBy.Equals("rocket")) {
            _levelController.IncreaseScore(10);
        } else {
            Debug.Log("Bad killedBy variable: " + killedBy);
        }
        gameObject.SetActive(false); //deactivate game object
    }
	*/

    private IEnumerator RedFlash() {
        Material enemyMaterial =enemyVisuals.GetComponent<Renderer>().material;
        Color originalColor = enemyMaterial.color;
        enemyMaterial.color = Color.red; //set the enemy color to red
        yield return new WaitForSeconds(0.15f); //wait
        enemyMaterial.color = originalColor; //set back to original color
    }
}