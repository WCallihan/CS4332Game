using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaveSpawner : MonoBehaviour {

    [Header("Enemy Waves")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private ParticleSystem enemySpawnParticles;
    [SerializeField] private AudioClip enemySpawnSound;
    [SerializeField] private int minEnemies;
    [SerializeField] private int maxEnemies;

    [Header("Boss Wave")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private int bossRoundFrequency; //a boss will spawn every x rooms

    [Header("Spawning Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    private AudioSource audioSource;
    private PlayerScore player;
	private int currentWave;
	private int totalWaveEnemies;
	private int waveEnemiesKilled;

	public event Action AllEnemiesDead;

	private void Awake() {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerScore>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
		currentWave = 0;
		waveEnemiesKilled = 0;
		StartCoroutine(SpawnWaveDelay(isBossRoom: false));
	}

	private void SpawnNewWave() {
		currentWave++;
        totalWaveEnemies = UnityEngine.Random.Range(minEnemies, maxEnemies + 1); //get how many enemies will be spawned
        waveEnemiesKilled = 0;

        for(int i=0; i<totalWaveEnemies; i++) {
            Vector3 spawnPos = GetMeshPoint() + new Vector3(0, 2, 0);
            StartCoroutine(SpawnEnemy(enemyPrefab, spawnPos));
        }
        if(enemySpawnSound) audioSource.PlayOneShot(enemySpawnSound);
        Debug.Log("Spawning " + totalWaveEnemies + " Enemies");
	}

    private void SpawnBoss() {
        currentWave++;
        totalWaveEnemies = 1;
        waveEnemiesKilled = 0;

        Vector3 spawnPos = GetMeshPoint() + new Vector3(0, 2.5f, 0);
        StartCoroutine(SpawnEnemy(bossPrefab, spawnPos));
        if(enemySpawnSound) audioSource.PlayOneShot(enemySpawnSound);
        Debug.Log("Spawning Boss");
    }

    //helper function to get the closest point on the nav mesh to spawn the enemy
    private Vector3 GetMeshPoint() {
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
        return spawnPos;
    }

    private IEnumerator SpawnEnemy(GameObject enemy, Vector3 spawnPos) {
        //spawn the particle system where the enemy will spawn
        if(enemySpawnParticles) Instantiate(enemySpawnParticles, spawnPos, enemySpawnParticles.transform.rotation);
        //wait a fourth the length of the particles
        yield return new WaitForSeconds(enemySpawnParticles.main.startLifetime.constant / 4f);
        //spawn the enemy and subscribe the function to its death event
        var spawnedEnemy = Instantiate(enemy, spawnPos, enemyPrefab.transform.rotation);
        spawnedEnemy.GetComponent<EnemyHealth>().EnemyDied += OnEnemyDeath;
    }

	private void OnEnemyDeath() {
		waveEnemiesKilled++;

		//check to see if the wave is over
		if(waveEnemiesKilled >= totalWaveEnemies) {
			//since the wave is over, spawn the next one if there is one
			if(currentWave == 1) {
                if((player.Score + 1) % bossRoundFrequency == 0) {
                    //boss round is up, spawn a boss
                    StartCoroutine(SpawnWaveDelay(isBossRoom: true));
                } else {
                    //no boss, spawn a normal wave
                    StartCoroutine(SpawnWaveDelay(isBossRoom : false));
                }
			} else if(currentWave == 2) {
				AllEnemiesDead?.Invoke();
			} else {
				Debug.LogError("currentWave has invalid value:" + currentWave);
			}
		}
	}

	//adds small delay between enemy waves
	private IEnumerator SpawnWaveDelay(Boolean isBossRoom) {
		yield return new WaitForSeconds(2);
        if(isBossRoom) {
            SpawnBoss();
        } else {
            SpawnNewWave();
        }
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