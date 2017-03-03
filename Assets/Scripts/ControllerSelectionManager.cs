using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectionManager : MonoBehaviour {
	private ViveController controller;
	private ushort vibrationForce = 3000;
	public GameObject targetObject;

	void Start () {
		controller = gameObject.transform.parent.GetComponent<ViveController> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Image") {
			controller.Vibrate (vibrationForce);
			targetObject = other.gameObject;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.GetComponent<LayerImage> ()) {
			other.gameObject.GetComponent<LayerImage> ().isHovered = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.GetComponent<LayerImage> ()) {
			other.gameObject.GetComponent<LayerImage> ().isHovered = false;
		}

		if (other.gameObject == targetObject) {
			targetObject = null;
		}
	}

	public void OnViveControllerTrigger(){
		Debug.Log ("Vive controller trigger");
		if (targetObject) {
			LayerImage layerImage = targetObject.gameObject.GetComponent<LayerImage> ();
			layerImage.ToggleSelection ();
		}
	}
}
