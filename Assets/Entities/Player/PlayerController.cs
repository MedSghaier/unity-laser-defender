using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float padding =1f ; 
	public float speed = 15.0f;
	public GameObject procjectile ; 
	public float projectileSpeed ;
	public float firingRate  =0.2f;
	public float health = 250f ;
	public AudioClip FiringSound ;
	public AudioClip destroyedSFX ; 
	public AudioClip hit;  

	private LevelManager lvlManager ;

	float Xmin ;
	float Xmax ;



	// Use this for initialization
	void Start () {
		lvlManager = GameObject.FindObjectOfType<LevelManager>();

	float distance = transform.position.z - Camera.main.transform.position.z;
	Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
	Vector3 righttMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
	Xmin = leftMost.x + padding;
	Xmax = righttMost.x - padding; 

	}
	
	// Update is called once per frame



	void Fire ()
	{
		GameObject beam = Instantiate (procjectile, transform.position, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, projectileSpeed, 0); 

		AudioSource.PlayClipAtPoint(FiringSound, transform.position);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}
		Move();
	}

	void Move ()
	{
		if (Input.GetKey (KeyCode.RightArrow)) {
			this.transform.position +=  Vector3.right * speed * Time.deltaTime ;

		}else if (Input.GetKey(KeyCode.LeftArrow)){
			this.transform.position += Vector3.left * speed * Time.deltaTime  ;
		}

		float newX = Mathf.Clamp(transform.position.x , Xmin, Xmax);
		transform.position = new Vector3 ( newX , transform.position.y, 0f);
}

	void OnTriggerEnter2D (Collider2D collider)
	{
		EnemyLaser missile = collider.gameObject.GetComponent<EnemyLaser> (); 
		if (missile) {
			Debug.Log("hit");
			AudioSource.PlayClipAtPoint(hit, transform.position);
			health -= missile.getDamage ();
			missile.Hit();
			if (health <= 0) {
			Destroy(gameObject);
			AudioSource.PlayClipAtPoint(destroyedSFX, transform.position);
			lvlManager.LoadLevel("Lose Screen");
			}
		}
	}
}
