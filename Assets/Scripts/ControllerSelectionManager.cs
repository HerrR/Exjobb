using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectionManager : MonoBehaviour {
	private ViveController controller;
	private LayerManager layerManager;
	private ushort vibrationForce = 3000;

	public GameObject targetObject;
	public GameObject triggerDownObject;

	private bool triggerContinuousPressLogged = false;

	void Start () {
		controller = gameObject.transform.parent.GetComponent<ViveController> ();
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
	}

	void OnTriggerEnter(Collider other) {
		if (layerManager.rearrangementMode)
			return;

		if (other.gameObject.tag == "Image") {
			if (Settings.selectionMode == "Gaze") {
				return;
			}

			if (!GameObject.Equals (other.gameObject, targetObject)) {
				controller.Vibrate (vibrationForce);
				targetObject = other.gameObject;
			}
		}

		if (other.gameObject.tag == "Lever") {
			controller.Vibrate (vibrationForce);
			targetObject = other.gameObject;
		}
	}

	void OnTriggerStay(Collider other) {
		if (layerManager.rearrangementMode)
			return;

		if (other.gameObject.GetComponent<LayerImage> ()) {
			if (targetObject == null)
				targetObject = other.gameObject;

			if (targetObject == other.gameObject) {
				if (!other.gameObject.GetComponent<LayerImage> ().isHovered)
					other.gameObject.GetComponent<LayerImage> ().ToggleHovered ();

			} else {
				if (other.gameObject.GetComponent<LayerImage> ().isHovered)
					other.gameObject.GetComponent<LayerImage> ().ToggleHovered ();
				
			}
		} else if (other.gameObject.GetComponent<Lever> ()) {
			if (!GameObject.Equals (targetObject, other.gameObject)) {
				targetObject = other.gameObject;
			}

			if (!other.gameObject.GetComponent<Lever> ().IsActive ()) {
				other.gameObject.GetComponent<Lever> ().ToggleActive ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (layerManager.rearrangementMode)
			return;

		if (other.gameObject.GetComponent<LayerImage> () && other.gameObject.GetComponent<LayerImage> ().isHovered)
			other.gameObject.GetComponent<LayerImage> ().ToggleHovered ();
		
		if (other.gameObject.GetComponent<Lever> () 
			&& other.gameObject.GetComponent<Lever> ().IsActive ()
			&& !GameObject.Equals(other.gameObject, triggerDownObject)) {
			other.gameObject.GetComponent<Lever> ().ToggleActive ();
		}

		if (other.gameObject == targetObject)
			targetObject = null;
	}

	public void OnViveControllerTrigger(){
		triggerContinuousPressLogged = false;
		
		if (targetObject) {
			if (targetObject.gameObject.GetComponent<Lever> ()) {
				controller.SetTriggerDownHoldTime (0.0f);
				triggerDownObject = targetObject;
				if (!targetObject.gameObject.GetComponent<Lever> ().IsActive ()) {
					targetObject.gameObject.GetComponent<Lever> ().ToggleActive ();
				}

				Logger.LogTrigger ("Lever");

			} else if (targetObject.gameObject.GetComponent<LayerImage> ()) {
				triggerDownObject = targetObject;

				if (Settings.selectionMode == "Gaze")
					return;
				
				targetObject.gameObject.GetComponent<LayerImage> ().ToggleSelection ();
				Logger.LogTrigger ("LayerImage");
			}
		} else {
			if (Settings.selectionMode == "Point") {
				Logger.LogTrigger ("No target");
			}
		}

	}

	void LogTriggerHold(string _target){
		string[] msg = {
			"$TriggerHold \t-\t "+ _target +" \t-\t " + Logger.GenerateTimestamp()
		};
		Logger.WriteToLog (msg);
		triggerContinuousPressLogged = true;
	}

	public void OnViveControllerTriggerHold(Vector3 devicePosition) {
		if (!triggerDownObject) {
			if (Settings.selectionMode == "Point") {
				if(!triggerContinuousPressLogged){
					LogTriggerHold ("No target");
				}
			}
			return;
		}

		if (triggerDownObject.GetComponent<Lever> ()) {
			if (!triggerContinuousPressLogged) {
				LogTriggerHold ("Lever");
			}

			triggerDownObject.GetComponent<Lever> ().MoveLever (devicePosition);
		}

		if (Settings.selectionMode == "Gaze")
			return;

		if (triggerDownObject.GetComponent<LayerImage> ()) {
			/* 
			if (!layerManager.rearrangementMode)
				layerManager.ToggleRearrangementMode ();
			*/


			if (!triggerDownObject.GetComponent<LayerImage> ().isSelected)
				triggerDownObject.GetComponent<LayerImage> ().ToggleSelection ();

			layerManager.MoveLayer (triggerDownObject, devicePosition);

			if (!triggerContinuousPressLogged) {
				LogTriggerHold ("LayerImage");
			}
		}
	}

	public void OnViveControllerTriggerRelease() {
		/* 
		if (layerManager.rearrangementMode) {
			layerManager.ToggleRearrangementMode ();
		}
		*/
		controller.ResetTriggerDownHoldTime ();

		if (!triggerDownObject)
			return;

		if (triggerDownObject.GetComponent<Lever> ()) {
			triggerDownObject.GetComponent<Lever> ().ToggleActive ();
		}
		triggerDownObject = default(GameObject);
	}

	public void OnViveControllerTrackpadPress(Vector3 movementVector){
		layerManager.MoveSelectedImagesInPlane (new Vector2 (movementVector.x * 100, movementVector.y * 100));
	}

	public void OnViveControllerGrip() {
		string[] msg = {
			"$Grip \t-\t DeselectAll \t-\t " + Logger.GenerateTimestamp()	
		};
		Logger.WriteToLog (msg);

		layerManager.DeselectAll ();
	}

	public void OnViveControllerApplicationMenu() {
		layerManager.ToggleRearrangementMode ();
		/*
		string[] msg = {
			"$ApplicationMenu \t-\t DeselectAll \t-\t"	+ Logger.GenerateTimestamp()
		};
		Logger.WriteToLog(msg);
		*/
		// layerManager.DeselectAll ();
	}

	public void OnViveControllerTrackpad(Vector2 movementVector){
		
		// layerManager.MoveSelectedImagesInPlane (movementVector);
	}
}
