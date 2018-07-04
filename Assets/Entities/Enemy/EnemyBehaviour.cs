using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject projectile ; 
	public float projectileSpeed = 10f; 
	public float ShotsPerSeconds = 0.5f ; 
	public int scoreValue = 150;
	public AudioClip attack;
	public AudioClip destroyed;

	float health = 150f;

	private ScoreKeeper scoreKeeper ;



	void Start ()
	{
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();	
	}
	void Fire ()
	{	AudioSource.PlayClipAtPoint(attack, transform.position);
		GameObject beam = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -projectileSpeed, 0); 
	}
	void Update ()
	{
		float probability = ShotsPerSeconds * Time.deltaTime; 
		if (Random.value < probability) {
			Fire ();
		}

	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile> (); 
		if (missile) {
			health -= missile.getDamage ();
			missile.Hit ();
			if (health <= 0) {
				Destroy (gameObject);
				AudioSource.PlayClipAtPoint (destroyed, transform.position);
				scoreKeeper.Score (scoreValue);
				}
			}
		}
	}
