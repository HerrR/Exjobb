using UnityEngine;
using System.Collections;

public class camera_movement : MonoBehaviour {
	public float speed;

	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			gameObject.transform.Translate (Vector3.forward * speed);
		}

		if(Input.GetKey (KeyCode.S)) {
			gameObject.transform.Translate (Vector3.back * speed);
		}
	}
}
