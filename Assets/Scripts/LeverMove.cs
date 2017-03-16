using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMove : MonoBehaviour {
	public Zone movementZone;

	public void Move(float percentage){
		Vector3 pos = new Vector3 (
			gameObject.transform.position.x,
			gameObject.transform.position.y,
			Mathf.Lerp (movementZone.bounds.zMin, movementZone.bounds.zMax, percentage)
		);
		transform.position = pos;
	}
}
