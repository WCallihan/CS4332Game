using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaveSpawner : MonoBehaviour {

    [Header("Enemy Waves")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int minEnemies;
    [SerializeField] private int maxEnemies;

    [Header("Spawning Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

	private int currentWave;
	private int totalWaveEnemies;
	private int waveEnemiesKilled;

	public event Action AllEnemiesDead;

	private void Awake() {
		currentWave = 0;
		waveEnemiesKilled = 0;
		StartCoroutine(SpawnWaveDelay());
	}

	private void SpawnNewWave() {
		currentWave++;
        totalWaveEnemies = UnityEngine.Random.Range(minEnemies, maxEnemies + 1); //get how many enemies will be spawned
        waveEnemiesKilled = 0;

        for(int i=0; i<totalWaveEnemies; i++) {
            //find a random spawn position on the nav mesh
            Vector3 spawnPos = Vector3.zero;
            do {
                float randX = UnityEngine.Random.Range(minX, maxX);
                float randZ = UnityEngine.Random.Range(minZ, maxZ);
                NavMeshHit hit;
                if(NavMesh.SamplePosition(new(randX, 0, randZ), out hit, 1, NavMesh.AllAreas)) {
                    spawnPos = hit.position;
                }
            } while(spawnPos == Vector3.zero);
            //spawn the enemy and subscribe the function to its death event
            var enemy = Instantiate(enemyPrefab, spawnPos + new Vector3(0f, 2f, 0f), enemyPrefab.transform.rotation);
            enemy.GetComponent<EnemyHealth>().EnemyDied += OnEnemyDeath;
        }
        Debug.Log("Spawning " + totalWaveEnemies + " Enemies");
	}

	private void OnEnemyDeath() {
		waveEnemiesKilled++;

		//check to see if the wave is over
		if(waveEnemiesKilled >= totalWaveEnemies) {
			//since the wave is over, spawn the next one if there is one
			if(currentWave == 1) {
				StartCoroutine(SpawnWaveDelay());
			} else if(currentWave == 2) {
				AllEnemiesDead?.Invoke();
			} else {
				Debug.LogError("currentWave has invalid value:" + currentWave);
			}
		}
	}

	//adds small delay between enemy waves
	private IEnumerator SpawnWaveDelay() {
		yield return new WaitForSeconds(2);
		SpawnNewWave();
	}

    //draw a rectangle around the set spawning bounds
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        //define the corners
        Vector3 topLeft = new(minX, 0, maxZ);
        Vector3 topRight = new(maxX, 0, maxZ);
        Vector3 bottomRight = new(maxX, 0, minZ);
        Vector3 bottomLeft = new(minX, 0, minZ);
        //draw the rectangle
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}