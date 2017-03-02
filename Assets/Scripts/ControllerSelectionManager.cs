using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectionManager : MonoBehaviour {
	ViveController controller;
	private ushort vibrationForce = 3000;
	public GameObject hoveredObject;

	// Use this for initialization
	void Start () {
		controller = gameObject.transform.parent.GetComponent<ViveController> ();
		Debug.Log (hoveredObject);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Image") {
			// Debug.Log ("Collision detected!", other.gameObject);

			controller.Vibrate (vibrationForce);
			hoveredObject = other.gameObject;

			// if(other.gameObject.GetComponent<ImageSwitcher>()){
			//	ImageSwitcher imgSwitch = other.gameObject.GetComponent<ImageSwitcher> ();
			//	imgSwitch.isSelected = !imgSwitch.isSelected;
			// }
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == hoveredObject) {
			hoveredObject = null;
		}
	}

	public void OnViveControllerTrigger(){
		if (hoveredObject) {
			LayerImage layerImage = hoveredObject.gameObject.GetComponent<LayerImage> ();
			layerImage.ToggleSelection ();
		}
	}
}
