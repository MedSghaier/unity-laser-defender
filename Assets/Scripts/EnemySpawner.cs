using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 0.1f;
	public float spawnDelay = 0.5f; 

	private bool movingRight = true;
	private float xmax ;
	private float xmin ;


	// Use this for initialization
	void Start ()
	{	
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distanceToCamera));
		xmax = rightEdge.x;
		xmin = leftEdge.x;

		SpawnUntilFull ();
	}

	void Formation ()
	{
		foreach (Transform child in transform) {
		GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
		enemy.transform.parent = child;
		}
	}

	void SpawnUntilFull ()
	{
		Transform freePosition = NextFreePosition();
		if(freePosition){
		GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
		enemy.transform.parent = freePosition;
		}
		if (NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	public void OnDrawGizmos ()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (movingRight) {
			MoveRight ();  
		} else {
			MoveLeft ();
		}

		float rightEdgeOfFormation = transform.position.x + (width / 2);
		float leftEdgeOfFormation = transform.position.x - (width / 2);

		if (rightEdgeOfFormation > xmax) {
			movingRight = false;
		} else if (leftEdgeOfFormation < xmin) {
			movingRight = true;
		}

		if (AllMembersDead ()) {
			SpawnUntilFull ();
		}
	}

	Transform NextFreePosition ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0 ) {
				return childPositionGameObject ;
			}
		}
		 return null;
	}

	bool AllMembersDead ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
			return false ;
			}
		}
		return true ; 
	}

	void MoveRight ()
	{
		transform.position += Vector3.right * speed * Time.deltaTime;
	}

	void MoveLeft ()
	{
		transform.position += Vector3.left * speed * Time.deltaTime;

	}
}
