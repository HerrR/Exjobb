using UnityEngine;
using System.Collections;

public class camera_movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Camera movement initialized");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			gameObject.transform.Translate (Vector3.forward);
		}

		if(Input.GetKey (KeyCode.S)) {
			gameObject.transform.Translate (Vector3.back);
		}
	}
}
