using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour {
	public ImageHit triggerDownObject;
	public ImageHit currentTarget;
	public Vector3 canvasHitAt;
	public Vector3 editPlaneHitAt;
	public GameObject tempObjectHolder;

	private ControllerRay ray;
	private Crosshair crosshair;
	private LayerManager layerManager;
	private ViveController controller;
	private ushort vibrationForce = 3000;

	public Vector3 editStartingPoint;
	public Vector3 controllerPositionAtEditStart;

	private bool tempRearrangementMode;

	[System.Serializable]
	public struct ImageHit{
		public GameObject target;
		public Vector3 impactPoint;

		public ImageHit(GameObject _target, Vector3 _impactPoint){
			this.target = _target;
			this.impactPoint = _impactPoint;
		}
	}

	// Use this for initialization
	void Start () {
		ray = GetComponent<ControllerRay> ();
		controller = GetComponent<ViveController> ();
		crosshair = GameObject.FindGameObjectWithTag ("Crosshair").GetComponent<Crosshair>();
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
		tempObjectHolder = GameObject.FindGameObjectWithTag ("Temp");
	}
	
	// Update is called once per frame
	void Update () {
		if (Settings.selectionMode == "Pointer") {
			if (!MovingZ ()) {
				UpdateTarget ();
				if (HasTarget ()) {
					ray.UpdateRay (Vector3.Distance (transform.position, currentTarget.impactPoint));
				}
			} else {
				HideRay ();
				crosshair.HideCrosshair ();
			}
		} else {
			ClearTarget ();
			HideRay ();
			crosshair.HideCrosshair ();
		}
	}

	void ClearTarget(){
		currentTarget = default(ImageHit);
	}

	void HideRay(){
		ray.UpdateRay (0f);
	}

	public bool HasTarget(){
		return currentTarget.target != default(GameObject);
	}

	public bool HasTriggerDownTarget(){
		return triggerDownObject.target != default(GameObject);
	}

	public void OnControllerTrigger(){
		if (HasTarget ()) {
			triggerDownObject = currentTarget;
		}
	}

	public void OnControllerTriggerRelease(){
		if (HasTarget ()) {
			if (currentTarget.target.GetComponent<MenuOption> ()) {
				currentTarget.target.GetComponent<MenuOption> ().SelectOption ();
				return;
			}
		}

		if (!controller.gripDown) {
			SingleSelect ();
		} else {
			AdditiveSelection ();
		}
		triggerDownObject = default(ImageHit);
	}

	public void OnControllerTriggerHold(){
		if (HasTarget ()) {
			layerManager.DeselectAll ();
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				currentTarget.target.transform.parent.GetComponentInChildren<Frame> ().Select ();
			}
		}
	}

	public void OnControllerTrackpad(){
		layerManager.UpdateStartingPositions ();
		controllerPositionAtEditStart = controller.gameObject.transform.position;

		// If the pointer has an active target, the point of impact should be the editstartingpoint
		if(HasTarget()){
			editStartingPoint = currentTarget.impactPoint;
			CreateEditPlane (editStartingPoint);
			return;
		}

		// If the pointer has no target and the layers are not expanded (flat), the hit on the main 
		// canvas should be the editstartingpoint
		if(canvasHitAt != default(Vector3)){
			editStartingPoint = canvasHitAt;
			CreateEditPlane (canvasHitAt);
			return;
		}
	}

	void CreateEditPlane(Vector3 _center){
		tempObjectHolder.transform.position = _center;
		BoxCollider editPlane = tempObjectHolder.AddComponent<BoxCollider> ();
		editPlane.isTrigger = true;
		editPlane.size = new Vector3 (20, 20, 0.01f);
	}

	void DeleteEditPlane(){
		foreach (BoxCollider bc in tempObjectHolder.GetComponents<BoxCollider>()) {
			GameObject.Destroy (bc);
		}
	}

	public void OnControllerContinuousTrackpad(){
		if (MovingZ() && !layerManager.rearrangementMode) {
			tempRearrangementMode = true;
			layerManager.ToggleRearrangementMode ();
		}

		if (layerManager.HasSelectedFrames ()) {
			Vector3 diffVector = controller.gameObject.transform.position - controllerPositionAtEditStart;
			layerManager.MoveSelectedLayersInZ (diffVector.z * 2.5f);
		}

		if (layerManager.HasSelectedImages ()) {
			if (editPlaneHitAt == default(Vector3)) {
				return;
			}
			Vector3 diffVector = editPlaneHitAt - editStartingPoint;
			Vector2 movementVector = new Vector2 (diffVector.x, diffVector.y);
			layerManager.MoveSelectedImagesInPlane (movementVector);
		}

	}

	public void OnControllerTrackpadRelease(){
		DeleteEditPlane ();
		editStartingPoint = default(Vector3);
		controllerPositionAtEditStart = default(Vector3);
		if (layerManager.rearrangementMode) {
			layerManager.ExpandLayers ();	
		}
		if (tempRearrangementMode) {
			if (layerManager.rearrangementMode) {
				layerManager.ToggleRearrangementMode ();
			}
		}
		tempRearrangementMode = false;
	}

	void AdditiveSelection(){
		if (HasTarget ()) {
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				layerManager.DeselectFrames ();
				currentTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
			}

			if (currentTarget.target.GetComponent<Frame> ()) {
				layerManager.DeselectImages ();
				currentTarget.target.GetComponent<Frame> ().ToggleSelection ();
			}
			return;
		}

		if (HasTriggerDownTarget ()) {
			if (triggerDownObject.target.GetComponent<LayerImage> ()) {
				triggerDownObject.target.GetComponent<LayerImage> ().ToggleSelection ();
			}

			if (triggerDownObject.target.GetComponent<Frame> ()) {
				triggerDownObject.target.GetComponent<Frame> ().ToggleSelection ();
			}
			return;
		}
	}

	void SingleSelect(){
		layerManager.DeselectAll ();
		if (HasTarget ()) {
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				currentTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
			}

			if (currentTarget.target.GetComponent<Frame> ()) {
				currentTarget.target.GetComponent<Frame> ().Select ();
			}

			return;
		}

		if (HasTriggerDownTarget ()) {
			if (triggerDownObject.target.GetComponent<LayerImage> ()) {
				triggerDownObject.target.GetComponent<LayerImage> ().ToggleSelection ();
			}

			if (triggerDownObject.target.GetComponent<Frame> ()) {
				triggerDownObject.target.GetComponent<Frame> ().Select ();
			}
			return;
		}
	}

	public bool MovingZ(){
		return layerManager.HasSelectedFrames () && controller.trackpadDown;
	}

	void UpdateTarget(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		RaycastHit[] hits;

		hits = Physics.RaycastAll (transform.position, fwd, 20);
		bool canvasHit = false;
		bool editPlaneHit = false;
		List<ImageHit> imagesHit = new List<ImageHit>();
		List<ImageHit> framesHit = new List<ImageHit> ();
		// List<ImageHit> menuOptionsHit = new List<ImageHit> ();

		foreach (RaycastHit hit in hits) {
			if (hit.collider.gameObject.tag == "Image") {
				imagesHit.Add (new ImageHit (hit.collider.gameObject, hit.point));
				continue;
			}

			if (hit.collider.gameObject.tag == "Frame") {
				framesHit.Add(new ImageHit(hit.collider.gameObject, hit.point));
				continue;
			}

			if (hit.collider.gameObject.tag == "MainCanvas") {
				canvasHitAt = hit.point;
				canvasHit = true;
				continue;
			}

			if (hit.collider.gameObject.tag == "MenuOption") {
				currentTarget = new ImageHit (hit.collider.gameObject, hit.point);
				return;
			}

			if (hit.collider.gameObject.tag == "Temp") {
				editPlaneHitAt = hit.point;
				editPlaneHit = true;
				continue;
			}
		}

		if (!editPlaneHit) {
			editPlaneHitAt = default(Vector3);
		}

		if (!canvasHit) {
			crosshair.HideCrosshair ();
			ray.UpdateRay ();
			canvasHitAt = default(Vector3);
		} else {
			if (layerManager.HasSelectedFrames () && controller.trackpadDown) {
				crosshair.HideCrosshair ();
				HideRay ();
			}
		}

		List<Layer> sortedLayers = layerManager.GetLayers ();
		bool imageFound = false;
		foreach (Layer layer in sortedLayers) {
			foreach (ImageHit imgHit in imagesHit) {
				if (layer.layerImage == imgHit.target.GetComponent<Image>()){
					imageFound = true;
					if (currentTarget.target != imgHit.target) {
						controller.Vibrate (vibrationForce);
					}
					currentTarget = imgHit;
					break;
				}
			}
			if (imageFound) {
				break;
			}
		}

		// If no image have been hit but a frame has, take the first frame hit and set it as the current target
		if(!imageFound){
			if (framesHit.Count > 0 && layerManager.rearrangementMode) {
				currentTarget = framesHit [0];	
			} else {
				currentTarget = default(ImageHit);
			}
		}

		if (!HasTarget () && canvasHit) {
			ray.UpdateRay (Vector3.Distance (transform.position, canvasHitAt));
			crosshair.ShowCrosshair ();
			crosshair.MoveCrosshair (canvasHitAt);
		}

		if (HasTarget ()) {
			crosshair.HideCrosshair ();
		}
	}
}
