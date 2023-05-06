using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private GameObject rocketArt;
    [SerializeField] private AudioClip explosionSound;

    private Rigidbody rocketRb;
    private AudioSource audioSource;
    private int damage;
    private float force;
    private float radius;
    private Vector3 moveVector;

    void Awake() {
        rocketRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        //always move in the direction of the move vector at a constant speed
        rocketRb.velocity = moveVector.normalized * Time.deltaTime * speed;
    }

    public void SetRocket(int damage, float force, float radius, Vector3 move) {
        this.damage = damage;
        this.force = force;
        this.radius = radius;
        moveVector = move;
    }

    private void OnCollisionEnter(Collision collision) {
        //get all enemies within the explosion radius
        Collider[] enemiesToDamage = Physics.OverlapSphere(transform.position, radius);
        //apply explosion force to all enemies
        foreach(Collider enemy in enemiesToDamage) {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damage); //apply damage to enemy
            enemyHealth?.gameObject.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius, 0, ForceMode.Impulse); //apply force to enemy
        }
		//spawn explosion particle effect
		if(explosionPrefab) {
			ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
			var exShp = explosion.shape;
			exShp.radius = radius - 3; //assign particle explosion radius to rocket explosion radius with arbitrary offset to make visual fit better
		}
        //play explosion sound and turn off visuals and collider
        if(explosionSound) audioSource.PlayOneShot(explosionSound);
        rocketArt.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        //despawn the rocket after the sound has finished playing
        StartCoroutine(DelayDeactivation());
    }

    private IEnumerator DelayDeactivation() {
        yield return new WaitWhile(() => audioSource.isPlaying); //wait until the sound has finished
        Destroy(gameObject);
    }
}