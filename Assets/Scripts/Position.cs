using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {

	void OnDrawGizmos ()
	{
		Gizmos.DrawSphere(transform.position, 1);
	}
}
