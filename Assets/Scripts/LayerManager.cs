using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LayerManager : MonoBehaviour {
	public bool rearrangementMode;
	public List<Layer> layers;
	public Pointer pointer;
	public ControllerSelectionManager controllerSelectionManager;

	void Start () {
		rearrangementMode = true;
	}

	void Update(){
		UpdateHovered ();
	}

	public void SortLayers(){
		layers.Sort (Layer.SortLayerByBasePosZ);
	}

	public List<Layer> GetLayers(){
		return layers;
	}

	// Called from Composite Image when all the layers have been created
	public void FindLayers(){
		layers = GameObject.FindObjectsOfType<Layer> ().ToList();
	}

	public void ToggleRearrangementMode(){
		if (!rearrangementMode) {
			ExpandLayers();
		} else {
			CollapseLayers();
		}
	
		rearrangementMode = !rearrangementMode;
	}

	public void ExpandLayers(){
		foreach (Layer layer in layers) {
			layer.CallMoveFromTo (
				layer.gameObject.transform, 
				layer.gameObject.transform.position,
				layer.basePosition,
				layer.movementSpeed);
				
		}
	}

	public void CollapseLayers(){
		foreach (Layer layer in layers) {
			Vector3 targetPos = new Vector3 ();
			targetPos.x = layer.gameObject.transform.position.x;
			targetPos.y = layer.gameObject.transform.position.y;
			targetPos.z = layer.zMax;

			layer.CallMoveFromTo (
				layer.gameObject.transform,
				layer.gameObject.transform.position,
				targetPos,
				layer.movementSpeed
			);
		}
	}

	void UpdateHovered(){
		if (Settings.selectionMode == "Pointer") {
			UpdatePointerHover ();
		} else if (Settings.selectionMode == "Direct") {
			UpdateControllerHover ();
		} else {
			Debug.LogError ("Unknown selection mode " + Settings.selectionMode);
		}
	}

	void UpdatePointerHover(){
		// If the pointer has no target -> Un-hover all layer images
		if (!pointer.HasTarget ()) {
			foreach (Layer layer in layers) {
				if (layer.GetComponentInChildren<LayerImage> ().isHovered) {
					layer.GetComponentInChildren<LayerImage> ().ToggleHovered ();
				}
			}
			return;
		}

		// If the pointer has a layer image target -> Mark pointer target as hovered and un-hover all else
		if (pointer.currentTarget.target.GetComponent<LayerImage> ()) {
			foreach (Layer layer in layers) {
				if (layer.GetComponentInChildren<LayerImage> () == pointer.currentTarget.target.GetComponent<LayerImage> ()) {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (true);
				} else {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (false);
				}
			}
		}
	}

	void UpdateControllerHover(){
		// TODO : Update the hovered status of objects depending on if they are being hovered by the controller
	}

	public void MoveLayer(GameObject layerGameObject, Vector3 _targetPosition) {
		try{
			layerGameObject.transform.parent.GetComponent<Layer>().MoveZ(_targetPosition.z);
		} catch {
			Debug.Log ("Couldn't move layer");
		}
	}

	public void DeselectAll() {
		// TODO: Update to use layers variable
		GameObject[] allImages = GameObject.FindGameObjectsWithTag ("Image");

		foreach (GameObject img in allImages) {
			if (!img.GetComponent<LayerImage> ()) {
				Debug.LogError ("Missing LayerImage component on object with Image tag:" + img.name, img);
				continue;
			}

			if (img.GetComponent<LayerImage> ().isSelected) {
				img.GetComponent<LayerImage> ().ToggleSelection ();
			}

			if (img.GetComponent<LayerImage> ().isHovered) {
				img.GetComponent<LayerImage> ().ToggleHovered ();
			}
		}
	}

	public void UpdateStartingPositions(){
		GameObject[] allImages = GameObject.FindGameObjectsWithTag ("Image");
		foreach (GameObject img in allImages) {
			img.GetComponent<LayerImage> ().UpdateStartingPosition ();
		}
	}

	public void MoveSelectedImagesInPlane(Vector2 movementVector){
		GameObject[] allImages = GameObject.FindGameObjectsWithTag ("Image");

		foreach(GameObject img in allImages) {
			if (!img.GetComponent<LayerImage> ()) {
				Debug.LogError ("Missing LayerImage component on object with Image tag:" + img.name, img);
				continue;
			}

			if (img.GetComponent<LayerImage> ().isSelected) 
				img.GetComponent<LayerImage> ().MoveImageByVector (movementVector);
		}
	}
}
