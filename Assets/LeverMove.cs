using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMove : MonoBehaviour {
	public Zone movementZone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move(float percentage){
		Vector3 pos = new Vector3 (
			gameObject.transform.position.x,
			gameObject.transform.position.y,
			Mathf.Lerp (movementZone.bounds.zMin, movementZone.bounds.zMax, percentage)
		);
		transform.position = pos;
	}
}
