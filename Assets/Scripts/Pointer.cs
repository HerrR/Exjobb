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

	public Vector3 editStartingPoint;

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
			UpdateTarget ();
		} else {
			ClearTarget ();
			HideRay ();
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
		if (!controller.gripDown) {
			SingleSelect ();
		} else {
			AdditiveSelection ();
		}
		triggerDownObject = default(ImageHit);
	}

	public void OnControllerTrackpad(){
		layerManager.UpdateStartingPositions ();

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
		if (editPlaneHitAt == default(Vector3)) {
			return;
		}
		Vector3 diffVec = editPlaneHitAt - editStartingPoint;
		Vector2 movementVector = new Vector2 (diffVec.x, diffVec.y);
		layerManager.MoveSelectedImagesInPlane (movementVector);
	}

	public void OnControllerTrackpadRelease(){
		DeleteEditPlane ();
		editStartingPoint = default(Vector3);
	}

	void AdditiveSelection(){
		if (HasTarget ()) {
			if (currentTarget.target.GetComponent<LayerImage> ()) {
				currentTarget.target.GetComponent<LayerImage> ().ToggleSelection ();
			}
			return;
		}

		if (HasTriggerDownTarget ()) {
			if (triggerDownObject.target.GetComponent<LayerImage> ()) {
				triggerDownObject.target.GetComponent<LayerImage> ().ToggleSelection ();
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
			return;
		}

		if (HasTriggerDownTarget ()) {
			if (triggerDownObject.target.GetComponent<LayerImage> ()) {
				triggerDownObject.target.GetComponent<LayerImage> ().ToggleSelection ();
			}
			return;
		}
	}

	void UpdateTarget(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		RaycastHit[] hits;

		hits = Physics.RaycastAll (transform.position, fwd, 20);
		bool canvasHit = false;
		bool editPlaneHit = false;
		List<ImageHit> imagesHit = new List<ImageHit>();

		foreach (RaycastHit hit in hits) {
			if (hit.collider.gameObject.tag == "Image") {
				imagesHit.Add (new ImageHit (hit.collider.gameObject, hit.point));
			}

			if (hit.collider.gameObject.tag == "MainCanvas") {
				crosshair.ShowCrosshair ();
				crosshair.MoveCrosshair (hit.point);
				canvasHitAt = hit.point;
				canvasHit = true;
			}

			if (hit.collider.gameObject.tag == "Temp") {
				editPlaneHitAt = hit.point;
				editPlaneHit = true;
			}

			if (hit.collider.gameObject.GetComponent<Zone> ()) {
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
			ray.UpdateRay (Vector3.Distance (transform.position, canvasHitAt));
		}

		List<Layer> sortedLayers = layerManager.GetLayers ();
		bool imageFound = false;
		foreach (Layer layer in sortedLayers) {
			foreach (ImageHit imgHit in imagesHit) {
				if (layer.layerImage == imgHit.target.GetComponent<Image>()){
					// .GetComponent<Image> ()) {
					imageFound = true;
					currentTarget = imgHit;
					break;
				}
			}
			if (imageFound) {
				break;
			}
		}

		if (!imageFound) {
			currentTarget = default(ImageHit);
		}
	}
}
