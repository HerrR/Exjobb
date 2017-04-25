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

	public void RearrangeShadowImages(){
		ShadowImage[] shadowImages = gameObject.GetComponentsInChildren<ShadowImage> ();
		for (int i = layers.Count - 1; i >= 0; i--) {
			layers [i].gameObject.transform.SetSiblingIndex (i+2);
			foreach (ShadowImage shadowImage in shadowImages) {
				if (layers [i].gameObject.GetComponentInChildren<LayerImage> ().GetComponent<Image> () == shadowImage.trackedImage) {
					shadowImage.gameObject.transform.SetAsLastSibling ();
				}
			}
		}
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
			Logger.LogExpandCollapse ("Expand");
			ExpandLayers();
		} else {
			Logger.LogExpandCollapse ("Collapse");
			CollapseLayers();
		}
	}

	public void ExpandLayers(){
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			layer.CallMoveFromTo (
				layer.gameObject.transform, 
				layer.gameObject.transform.position,
				layer.basePosition,
				layer.movementSpeed);
				
		}
		rearrangementMode = true;
	}

	public void CollapseLayers(){
		RearrangeShadowImages ();
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
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
		rearrangementMode = false;
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
			for(int i = 0 ; i < layers.Count ; i++){
				Layer layer = layers [i];
				if (layer.GetComponentInChildren<LayerImage> ().isHovered) {
					layer.GetComponentInChildren<LayerImage> ().ToggleHovered ();
				}

				if (layer.GetComponentInChildren<Frame> ().IsHovered ()) {
					layer.GetComponentInChildren<Frame> ().Unhover ();
				}
			}
			return;
		}

		// If the pointer has a layer image target -> Mark pointer target as hovered and un-hover all else
		if (pointer.currentTarget.target.GetComponent<LayerImage> ()) {

			for(int i = 0 ; i < layers.Count ; i++){
				Layer layer = layers [i];
				if (layer.GetComponentInChildren<LayerImage> () == pointer.currentTarget.target.GetComponent<LayerImage> ()) {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (true);
				} else {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (false);
				}
			}
		}

		// If the pointer has a frame target -> Mark frame as hovered an un-hover all else
		if(pointer.currentTarget.target.GetComponent<Frame>()){
			for (int i = 0; i < layers.Count; i++) {
				Layer layer = layers [i];
				if (layer.GetComponentInChildren<Frame> () == pointer.currentTarget.target.GetComponent<Frame> ()) {
					layer.GetComponentInChildren<Frame> ().Hover ();
				} else {
					layer.GetComponentInChildren<Frame> ().Unhover ();
				}
			}
		}
	}

	void UpdateControllerHover(){
		if(!controllerSelectionManager.HasTarget()){
			for(int i = 0 ; i < layers.Count ; i++){
				Layer layer = layers [i];
				if (layer.GetComponentInChildren<LayerImage> ().isHovered) {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (false);
				}

				if (layer.GetComponentInChildren<Frame> ().IsHovered()) {
					layer.GetComponentInChildren<Frame> ().Unhover ();
				}
			}
			return;
		}

		if (controllerSelectionManager.currentTarget.target.GetComponent<LayerImage> ()) {
			//foreach (Layer layer in layers) {
			for(int i = 0 ; i < layers.Count ; i++){
				Layer layer = layers [i];
				if (layer.GetComponentInChildren<LayerImage> () == controllerSelectionManager.currentTarget.target.GetComponent<LayerImage> ()) {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (true);
				} else {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (false);
					layer.GetComponentInChildren<Frame> ().Unhover ();
				}
			}
			return;
		}

		if (controllerSelectionManager.currentTarget.target.GetComponent<Frame> ()) {
			//foreach (Layer layer in layers) {
			for(int i = 0 ; i < layers.Count ; i++){
				Layer layer = layers [i];
				if (layer.GetComponentInChildren<Frame> () == controllerSelectionManager.currentTarget.target.GetComponent<Frame> ()) {
					layer.GetComponentInChildren<Frame> ().Hover ();
				} else {
					layer.GetComponentInChildren<LayerImage> ().SetHovered (false);
					layer.GetComponentInChildren<Frame> ().Unhover ();
				}
			}
		}
	}

	public void MoveLayer(GameObject layerGameObject, Vector3 _targetPosition) {
		try{
			layerGameObject.transform.parent.GetComponent<Layer>().MoveZ(_targetPosition.z);
		} catch {
			Debug.Log ("Couldn't move layer");
		}
	}

	public void MoveSelectedLayersInZ(float _diffZ){
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			if (layer.GetComponentInChildren<Frame> ().IsSelected ()) {
				layer.MoveZ (layer.startEditPosition.z + _diffZ);
			}
		}
	}

	public bool HasSelectedFrames(){
		bool found = false;
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			if (layer.GetComponentInChildren<Frame> ().IsSelected ()) {
				found = true;
				break;
			}
		}
		return found;
	}

	public bool HasSelectedImages(){
		bool found = false;
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			if (layer.GetComponentInChildren<LayerImage> ().isSelected) {
				found = true;
				break;
			}
		}
		return found;
	}

	public void DeselectAll() {
		DeselectFrames();
		DeselectImages ();
	}

	public void DeselectFrames(){
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			layer.GetComponentInChildren<Frame> ().Deselect ();
		}
	}

	public void DeselectImages(){
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			layer.GetComponentInChildren<LayerImage> ().SetSelected (false);
		}
	}

	public void UpdateStartingPositions(){
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			layer.GetComponentInChildren<LayerImage> ().UpdateStartingPosition ();
			layer.UpdateStartEditPosition ();
		}
	}

	public void MoveSelectedImagesInPlane(Vector2 movementVector){
		for(int i = 0 ; i < layers.Count ; i++){
			Layer layer = layers [i];
			if (layer.GetComponentInChildren<LayerImage> ().isSelected) {
				layer.GetComponentInChildren<LayerImage> ().MoveImageByVector (movementVector);
			}
		}
	}

	public int NumSelectedImages(){
		int numSelected = 0;
		for (int i = 0; i < layers.Count; i++) {
			if (layers[i].GetComponentInChildren<LayerImage> ().isSelected) {
				numSelected++;
			}
		}
		return numSelected;
	}

	public int NumSelectedFrames(){
		int numSelected = 0;
		for (int i = 0; i < layers.Count; i++) {
			if (layers [i].GetComponentInChildren<Frame> ().IsSelected ()) {
				numSelected++;
			}
		}
		return numSelected;
	}
}
