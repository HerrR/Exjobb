using UnityEngine;
using System.Collections;

public class camera_movement : MonoBehaviour {
	public float speed;

	void Start(){
		if (speed == 0f) {
			speed = 0.1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			gameObject.transform.Translate (Vector3.forward * speed * Time.deltaTime);
		}

		if(Input.GetKey (KeyCode.S)) {
			gameObject.transform.Translate (Vector3.back * speed * Time.deltaTime);
		}
	}
}
