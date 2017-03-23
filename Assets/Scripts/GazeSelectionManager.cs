using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelectionManager : MonoBehaviour {
	public ViveController[] controllers;
	private LayerManager layerManager;
	private GazeTracker gazeTracker;
	public GameObject triggerDownObject;

	// Use this for initialization
	void Start () {
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
		gazeTracker = gameObject.GetComponent<GazeTracker> ();
	}

	void FindControllers(){
		controllers = GameObject.FindObjectsOfType<ViveController> ();	
	}

	public void OnViveControllerTrigger(){
		if (Settings.selectionMode != "Gaze")
			return;
		
		if (gazeTracker.HasTarget ()) {
			if (gazeTracker.currentTarget.GetComponent<LayerImage> ()) {
				gazeTracker.currentTarget.GetComponent<LayerImage> ().ToggleSelection ();
				triggerDownObject = gazeTracker.currentTarget;
			}
		}
	}

	public void OnViveControllerTriggerHold(Vector3 devicePosition){
		if (Settings.selectionMode != "Gaze")
			return;

		if (!triggerDownObject)
			return;

		FindControllers ();

		foreach (ViveController controller in controllers) {
			if (controller.controllerSelectionManager.triggerDownObject) {
				if (controller.controllerSelectionManager.triggerDownObject.GetComponent<Lever> ()) {
					return;
				}
			}
		}
		
		if (triggerDownObject.GetComponent<LayerImage> ()) {
			if (!layerManager.rearrangementMode)
				layerManager.ToggleRearrangementMode ();

			if (!triggerDownObject.GetComponent<LayerImage> ().isSelected)
				triggerDownObject.GetComponent<LayerImage> ().ToggleSelection ();

			layerManager.MoveLayer (triggerDownObject, devicePosition);
		}
	}

	public void OnViveControllerTriggerRelease(){
		if (Settings.selectionMode != "Gaze")
			return;
		
		triggerDownObject = default(GameObject);
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
