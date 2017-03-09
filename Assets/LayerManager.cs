using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerManager : MonoBehaviour {
	public bool rearrangementMode;

	// Use this for initialization
	void Start () {
		rearrangementMode = false;
	}

	public void ToggleRearrangementMode(){
		if (!rearrangementMode) {
			DisableAccordionEffect ();
			// LayersToOriginalPositions ();
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

		layerGameObject.transform.parent.GetComponent<Layer>().MoveZ(_targetPosition.z);
		/*
		try{
		} catch {
			Debug.Log ("Couldn't move layer");
		}
		*/
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

	public void LayersToOriginalPositions(){
		foreach(GameObject img in GameObject.FindGameObjectsWithTag ("Image")) {
			try{
				img.transform.parent.position = img.transform.parent.gameObject.GetComponent<Layer>().basePosition;	
			} catch {
				Debug.LogError ("Failed to update original position ", img);
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
}
