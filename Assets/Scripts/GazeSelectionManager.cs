using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelectionManager : MonoBehaviour {
	private ViveController[] controllers;
	private LayerManager layerManager;
	private GazeTracker gazeTracker;

	// Use this for initialization
	void Start () {
		controllers = GameObject.FindObjectsOfType<ViveController> ();	
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
		gazeTracker = gameObject.GetComponent<GazeTracker> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnViveControllerTrigger(){
		if (Settings.selectionMode != "Gaze")
			return;
		
		if (gazeTracker.HasTarget ()) {
			if (gazeTracker.currentTarget.GetComponent<LayerImage> ()) {
				gazeTracker.currentTarget.GetComponent<LayerImage> ().ToggleSelection ();
				// TODO: Trigger down object, here or in gaze tracker? Probably here.
			}
		}
	}

	public void OnViveControllerTriggerHold(Vector3 devicePosition){
		// Debug.Log ("Vive controller trigger hold called in gaze selection manager");
	}

	public void OnViveControllerTriggerRelease(){
		// Debug.Log ("Vive controller trigger release called in gaze selection manager");
	}

	public void OnViveControllerGrip(){
		// Debug.Log ("Vive controller grip called in gaze selection manager");
	}

	public void OnViveControllerApplicationMenu(){
		// Debug.Log ("Vive controller application menu called in gaze selection manager");
	}

	public void OnViveControllerTrackpad(Vector2 movementVector){
		// Debug.Log ("Vive controlelr trackpad called in gaze selection manager");
	}
}
