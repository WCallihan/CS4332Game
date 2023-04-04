using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour {

	[SerializeField] private GameObject[] enemyWave1;
	[SerializeField] private GameObject[] enemyWave2;

	private int currentWave;
	private int totalWaveEnemies;
	private int waveEnemiesKilled;

	private void Awake() {
		currentWave = 0;
		waveEnemiesKilled = 0;
		StartCoroutine(SpawnWaveDelay(enemyWave1));
	}

	private void SpawnNewWave(GameObject[] enemyWave) {
		currentWave++;
		totalWaveEnemies = enemyWave.Length;
		waveEnemiesKilled = 0;
		foreach(var e in enemyWave) {
			e.GetComponent<EnemyHealth>().EnemyDied += OnEnemyDeath;
			e.SetActive(true);
		}
	}

	private void OnEnemyDeath() {
		waveEnemiesKilled++;

		//check to see if the wave is over
		if(waveEnemiesKilled >= totalWaveEnemies) {
			//since the wave is over, spawn the next one if there is one
			if(currentWave == 1) {
				StartCoroutine(SpawnWaveDelay(enemyWave2));
			}
		}
	}

	//adds small delay between enemy waves
	private IEnumerator SpawnWaveDelay(GameObject[] enemyWave) {
		yield return new WaitForSeconds(2);
		SpawnNewWave(enemyWave);
	}
}