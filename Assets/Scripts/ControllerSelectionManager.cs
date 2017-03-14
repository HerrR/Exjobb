using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelectionManager : MonoBehaviour {
	private ViveController controller;
	private LayerManager layerManager;
	private ushort vibrationForce = 3000;
	public GameObject targetObject;
	public GameObject triggerDownObject;
	public bool arrangingLayers;

	void Start () {
		controller = gameObject.transform.parent.GetComponent<ViveController> ();
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
		arrangingLayers = false;
	}

	void OnTriggerEnter(Collider other) {
		if (layerManager.rearrangementMode)
			return;

		if (other.gameObject.tag == "Image") {
			controller.Vibrate (vibrationForce);
			targetObject = other.gameObject;
		}
	}

	void OnTriggerStay(Collider other) {
		if (layerManager.rearrangementMode)
			return;

		if (other.gameObject.GetComponent<LayerImage> ()) {
			if(targetObject == null)
				targetObject = other.gameObject;

			if (targetObject == other.gameObject) {
				if (!other.gameObject.GetComponent<LayerImage> ().isHovered)
					other.gameObject.GetComponent<LayerImage> ().ToggleHovered ();

			} else {
				if (other.gameObject.GetComponent<LayerImage> ().isHovered) 
					other.gameObject.GetComponent<LayerImage> ().ToggleHovered ();
				
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (layerManager.rearrangementMode)
			return;

		if (other.gameObject.GetComponent<LayerImage> () && other.gameObject.GetComponent<LayerImage> ().isHovered)
			other.gameObject.GetComponent<LayerImage> ().ToggleHovered ();

		if (other.gameObject == targetObject)
			targetObject = null;
	}

	public void OnViveControllerTrigger(){
		if (targetObject) {
			targetObject.gameObject.GetComponent<LayerImage> ().ToggleSelection ();
			triggerDownObject = targetObject;
		}
	}

	public void OnViveControllerTriggerHold(Vector3 devicePosition) {
		if (!triggerDownObject)
			return;

		if (!layerManager.rearrangementMode)
			layerManager.ToggleRearrangementMode ();

		if (triggerDownObject)
			layerManager.MoveLayer (triggerDownObject, devicePosition);
	}

	public void OnViveControllerTriggerRelease() {
		if (layerManager.rearrangementMode)
			layerManager.ToggleRearrangementMode ();

		triggerDownObject = default(GameObject);
	}

	public void OnViveControllerGrip() {
		layerManager.DeselectAll ();	
	}

	public void OnViveControllerApplicationMenu() {
		layerManager.DeselectAll ();
	}

	public void OnViveControllerTrackpad(Vector2 movementVector){
		layerManager.MoveSelectedImagesInPlane (movementVector);
	}
}
