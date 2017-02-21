﻿using System.Collections;
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
	public GameObject generationZone;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		generationZone = GameObject.FindGameObjectWithTag ("GenerationZone");
	}

	void OnTriggerEnter(Collider other) {
		if (!other.gameObject.GetComponent<Zone> ()) 
			return;

		if (other.gameObject.GetComponent<Zone> ().type != "mode")
			return;

		switchMode (other.gameObject.GetComponent<Zone> ().name);

		if (textUpdateOnChange) {
			textUpdateOnChange.GetComponent<Text> ().text = other.gameObject.GetComponent<Zone>().name;
		}
	}

	void switchMode(string _mode){
		mode = _mode;

		switch (mode){
		case "overview":
			mainCanvas.GetComponent<CompositeImage> ().ShowFrame ();
			foreach(GameObject layer in GameObject.FindGameObjectsWithTag("Layer")){
				layer.GetComponent<Layer> ().copyToCanvas ();
				Destroy (layer);
			}
			break;
		case "inspection":
			float zMin = generationZone.GetComponent<Zone> ().bounds.zMin;
			float zMax = generationZone.GetComponent<Zone> ().bounds.zMax;
			mainCanvas.GetComponent<CompositeImage> ().GenerateLayers (zMin, zMax);
			mainCanvas.GetComponent<CompositeImage> ().ClearCanvas ();
			mainCanvas.GetComponent<CompositeImage> ().HideFrame ();
			break;
		default:
			break;
		}

	}
}