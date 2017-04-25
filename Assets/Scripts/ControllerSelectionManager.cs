using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectionManager : MonoBehaviour {
	private ViveController controller;
	private LayerManager layerManager;
	private ushort vibrationForce = 3000;

	public float amplificationFactor = 2.0f;

	public Target currentTarget;
	public Target triggerDownTarget;

	private bool triggerContinuousPressLogged = false;
	private bool tempRearrangementMode = false;
	public Vector3 editStartingPoint;

	private bool trackpadInteractionLogged = false;

	[System.Serializable]
	public struct Target{
		public GameObject target;
		public Vector3 impactPoint;

		public Target(GameObject _target, Vector3 _impactPoint){
			this.target = _target;
			this.impactPoint = _impactPoint;
		}
	}

	void Start () {
		controller = gameObject.transform.parent.GetComponent<ViveController> ();
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
	}

	void Update(){
		if (Settings.selectionMode != "Direct") {
			return;
		}

		if (HasTarget ()) {
			if (currentTarget.target.tag == "MenuOption") {
				if (!currentTarget.target.GetComponent<MenuOption> ().IsShowing ()) {
					ResetTargets ();
				}
			}
		}
	}

	public bool HasTarget(){
		return currentTarget.target != default(GameObject);
	}

	public bool HasTriggerDownTarget(){
		return triggerDownTarget.target != default(GameObject);
	}

	public bool MovingZ(){
		return layerManager.HasSelectedFrames () && controller.trackpadDown;
	}

	void OnTriggerEnter(Collider other) {
		if (Settings.selectionMode != "Direct") {
			return;
		}

		if (HasTarget ()) {
			return;
		}

		if (MovingZ ()) {
			return;
		}

		if (other.gameObject.tag == "Frame") {
			if (layerManager.rearrangementMode) {
				currentTarget = new Target (other.gameObject, controller.gameObject.transform.position);
				controller.Vibrate (vibrationForce);
				return;
			}
		}

		if (other.gameObject.tag == "Image") {
			currentTarget = new Target (other.gameObject, controller.gameObject.transform.position);
			controller.Vibrate (vibrationForce);
			return;
		}
	}

	void OnTriggerStay(Collider other) {
		if (MovingZ ()) {
			return;
		}

		if (!HasTarget ()) {
			if (other.gameObject.tag == "MenuOption") {
				currentTarget = new Target (other.gameObject, controller.gameObject.transform.position);
				controller.Vibrate (vibrationForce);
				return;
			}

			if (other.gameObject.tag == "Frame") {
				if (layerManager.rearrangementMode) {
					currentTarget = new Target (other.gameObject, controller.gameObject.transform.position);
					controller.Vibrate (vibrationForce);
					return;
				}
			}

			if (other.gameObject.tag == "Image") {
				currentTarget = new Target (other.gameObject, controller.gameObject.transform.position);
				controller.Vibrate (vibrationForce);
				return;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(GameObject.Equals(other.gameObject, currentTarget.target)){
			currentTarget = default(Target);
		}
	}

	public void OnViveControllerTrigger(){
		if (HasTarget ()) {
			triggerDownTarget = currentTarget;
		}
	}

	public void OnViveControllerTriggerHold() {
		if (HasTarget ()) {
			layerManager.DeselectAll ();
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				currentTarget.target.transform.parent.GetComponentInChildren<Frame> ().Select ();
			}
		}
	}

	void SingleSelect(){
		layerManager.DeselectAll ();
		if(HasTarget()){
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				currentTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
				Logger.LogSelection ("Single", "LayerImage");
			}

			if (currentTarget.target.GetComponent<Frame> ()) {
				currentTarget.target.GetComponent<Frame> ().ToggleSelection ();
				Logger.LogSelection ("Single", "Frame");
			}
			return;
		}

		if (HasTriggerDownTarget ()) {
			if (triggerDownTarget.target.GetComponent<LayerImage> ()) {
				triggerDownTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
				Logger.LogSelection ("Single", "LayerImage");
			}

			if (triggerDownTarget.target.GetComponent<Frame> ()) {
				triggerDownTarget.target.GetComponent<Frame> ().ToggleSelection ();
				Logger.LogSelection ("Single", "Frame");
			}
			return;
		}

		Logger.LogSelection ("Single", "No Target");

	}

	void AdditiveSelect(){
		if (HasTarget ()) {
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				layerManager.DeselectFrames ();
				currentTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
				Logger.LogSelection ("Additive", "LayerImage");
			}

			if (currentTarget.target.GetComponent<Frame> ()) {
				layerManager.DeselectImages ();
				currentTarget.target.GetComponent<Frame> ().ToggleSelection ();
				Logger.LogSelection ("Additive", "Frame");
			}
			return;
		}

		if (HasTriggerDownTarget ()) {
			if (triggerDownTarget.target.GetComponent<LayerImage> ()) {
				triggerDownTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
				Logger.LogSelection ("Additive", "LayerImage");
			}

			if (triggerDownTarget.target.GetComponent<Frame> ()) {
				triggerDownTarget.target.GetComponent<Frame> ().ToggleSelection ();
				Logger.LogSelection ("Additive", "Frame");
			}
			return;
		}

		Logger.LogSelection ("Additive", "No Target");
	}

	public void OnViveControllerTriggerRelease() {
		if (HasTarget ()) {
			if (currentTarget.target.GetComponent<MenuOption> ()) {
				currentTarget.target.GetComponent<MenuOption> ().SelectOption ();
				Logger.LogMenuInteraction (currentTarget.target.name);
				return;
			}
		}

		if (!controller.gripDown) {
			SingleSelect ();
		} else {
			AdditiveSelect ();
		}
		triggerDownTarget = default(Target);
	}

	public void OnViveControllerTrackpad(){
		layerManager.UpdateStartingPositions ();
		SetEditStartingPointToControllerPosition ();
		if (layerManager.HasSelectedFrames () && !layerManager.rearrangementMode) {
			layerManager.ToggleRearrangementMode ();
			tempRearrangementMode = true;
		}
	}

	public void SetEditStartingPointToControllerPosition(){
		editStartingPoint = controller.gameObject.transform.position;
		layerManager.UpdateStartingPositions ();
	}

	public void OnViveControllerTrackpadContinuous(){
		Vector3 diffVector = controller.gameObject.transform.position - editStartingPoint;

		if (layerManager.HasSelectedFrames ()) {
			if (!trackpadInteractionLogged) {
				Logger.LogInteraction ("MoveZ", layerManager.NumSelectedFrames ());
				trackpadInteractionLogged = true;
			}
			layerManager.MoveSelectedLayersInZ (diffVector.z * amplificationFactor);
		}

		if (layerManager.HasSelectedImages ()) {
			if (!trackpadInteractionLogged) {
				Logger.LogInteraction ("MoveXY", layerManager.NumSelectedImages ());
				trackpadInteractionLogged = true;
			}
			layerManager.MoveSelectedImagesInPlane (new Vector2(diffVector.x * amplificationFactor, diffVector.y * amplificationFactor));
		}
	}

	public void OnViveControllerTrackpadRelease(){
		editStartingPoint = default(Vector3);
		trackpadInteractionLogged = false;

		if (layerManager.rearrangementMode) {
			layerManager.ExpandLayers ();	
		}

		if (tempRearrangementMode && layerManager.rearrangementMode) {
			layerManager.ToggleRearrangementMode ();
		}

		tempRearrangementMode = false;
	}

	/*
	public void OnViveControllerApplicationMenu() {
		layerManager.ToggleRearrangementMode ();
	}
	*/

	public void ResetTargets(){
		currentTarget = default(Target);
		triggerDownTarget = default(Target);
	}
}
