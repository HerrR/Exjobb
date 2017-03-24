using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelectionManager : MonoBehaviour {
	public ViveController[] controllers;
	private LayerManager layerManager;
	private GazeTracker gazeTracker;
	public GameObject triggerDownObject;
	private bool triggerContinuousPressLogged = false;

	// Use this for initialization
	void Start () {
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
		gazeTracker = gameObject.GetComponent<GazeTracker> ();
	}

	void FindControllers(){
		controllers = GameObject.FindObjectsOfType<ViveController> ();	
	}

	void LogTriggerHold(string _target){
		string[] msg = {
			"$TriggerHold \t-\t "+ _target +" \t-\t " + Logger.GenerateTimestamp()
		};
		Logger.WriteToLog (msg);
		triggerContinuousPressLogged = true;
	}

	public void OnViveControllerTrigger(){
		triggerContinuousPressLogged = false;
		if (Settings.selectionMode != "Gaze")
			return;


		if (gazeTracker.HasTarget ()) {
			if (gazeTracker.currentTarget.GetComponent<LayerImage> ()) {
				gazeTracker.currentTarget.GetComponent<LayerImage> ().ToggleSelection ();
				triggerDownObject = gazeTracker.currentTarget;
				string[] msg = {
					"$Trigger \t-\t LayerImage \t-\t " + Logger.GenerateTimestamp()	
				};

				Logger.WriteToLog (msg);
			}
		} else {
			string[] msg = {
				"$Trigger \t-\t No target \t-\t " + Logger.GenerateTimestamp()	
			};

			Logger.WriteToLog (msg);
		}
	}

	public void OnViveControllerTriggerHold(Vector3 devicePosition){
		if (Settings.selectionMode != "Gaze")
			return;

		if (!triggerDownObject) {
			if (!triggerContinuousPressLogged) {
				LogTriggerHold ("No target");
			}
			return;
		}

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

			if (!triggerContinuousPressLogged) {
				LogTriggerHold ("LayerImage");
			}

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
