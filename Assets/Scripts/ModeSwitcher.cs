using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour {
	public string mode;

	// Assigned in inspector
	public GameObject textUpdateOnChange;

	// Auto assigned by tag
	public GameObject mainCanvas;

	// Auto assigned by tag
	public GameObject inspectionZone;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		inspectionZone = GameObject.FindGameObjectWithTag ("InspectionZone");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Zone> ()) {
			switchMode (other.gameObject.GetComponent<Zone>().name);
			if (textUpdateOnChange) {
				textUpdateOnChange.GetComponent<Text> ().text = other.gameObject.GetComponent<Zone>().name;
			} 
		}
	}

	void switchMode(string _mode){
		mode = _mode;

		switch (mode){
		case "overview":
			foreach(GameObject layer in GameObject.FindGameObjectsWithTag("Layer")){
				layer.GetComponent<Layer> ().copyToCanvas ();
				Destroy (layer);
			}
			break;
		case "inspection":
			float zMin = inspectionZone.GetComponent<Zone> ().bounds.zMin;
			float zMax = inspectionZone.GetComponent<Zone> ().bounds.zMax;
			mainCanvas.GetComponent<CompositeImage> ().GenerateLayers (zMin, zMax);
			mainCanvas.GetComponent<CompositeImage> ().ClearCanvas ();
			break;
		default:
			break;
		}

	}
}