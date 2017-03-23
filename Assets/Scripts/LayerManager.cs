using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LayerManager : MonoBehaviour {
	public bool rearrangementMode;

	// Use this for initialization
	void Start () {
		rearrangementMode = false;
	}

	public void ToggleRearrangementMode(){
		if (!rearrangementMode) {
			DisableAccordionEffect ();
		} else {
			EnableAccordionEffect ();
		}
	
		rearrangementMode = !rearrangementMode;
	}

	public void MoveLayer(GameObject layerGameObject, Vector3 _targetPosition) {
		if (!rearrangementMode) {
			ToggleRearrangementMode ();
			DeselectAll ();
			layerGameObject.GetComponent<LayerImage> ().ToggleSelection ();
		}

		try{
			layerGameObject.transform.parent.GetComponent<Layer>().MoveZ(_targetPosition.z);
		} catch {
			Debug.Log ("Couldn't move layer");
		}
	}

	public void DisableAccordionEffect() {
		foreach (GameObject img in GameObject.FindGameObjectsWithTag ("Image")) {
			try{
				if(img.transform.parent.GetComponentInChildren<Layer>().accordion){
					img.transform.parent.GetComponentInChildren<Layer>().ToggleAccordion();
				}
			} catch {
				Debug.LogError ("Failed to disable accordion effect", img);
			}
		}
	}

	public void EnableAccordionEffect() {
		foreach (GameObject img in GameObject.FindGameObjectsWithTag ("Image")) {
			try{
				if(!img.transform.parent.GetComponentInChildren<Layer>().accordion){
					img.transform.parent.GetComponentInChildren<Layer>().ToggleAccordion();
				}
			} catch {
				Debug.LogError ("Failed to enable accordion effect", img);
			}
		}
	}

	public void DeselectAll() {
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

	public void MoveSelectedImagesInPlane(Vector2 movementVector){
		GameObject[] allImages = GameObject.FindGameObjectsWithTag ("Image");

		foreach(GameObject img in allImages) {
			if (!img.GetComponent<LayerImage> ()) {
				Debug.LogError ("Missing LayerImage component on object with Image tag:" + img.name, img);
				continue;
			}

			if (img.GetComponent<LayerImage> ().isSelected) 
				img.GetComponent<LayerImage> ().MoveImage (movementVector);
		}
	}

	public void GenerateShadowImages(){
		List<Layer> allLayers = GameObject.FindObjectsOfType<Layer> ().ToList ();
		allLayers.Sort (Layer.SortLayerByBasePosZ);
		allLayers.Reverse ();

		foreach (Layer layer in allLayers) {
			if (layer.HasShadowImage ()) {
				layer.DestroyShadowImage ();
			}

			layer.GenerateShadowImages ();
		}
	}
}
