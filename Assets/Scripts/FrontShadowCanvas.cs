using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontShadowCanvas : MonoBehaviour {
	public Zone generationZone;
	// Use this for initialization
	void Start () {
		generationZone = GameObject.FindGameObjectWithTag ("GenerationZone").GetComponent<Zone>();
		MoveToPosition ();
	}

	void MoveToPosition(){
		Vector3 newPos = transform.position;
		newPos.z = generationZone.bounds.zMin;
		transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
