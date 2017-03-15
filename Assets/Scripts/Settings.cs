using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
	// public string selectionMode = "Point";
	// public string selectionMode = "Gaze";

	// public string navigationMode = "Spatial";
	// public string navigationMode = "Lever";
	public BoxCollider cameraMainCollider;
	public BoxCollider leverMovedObjectCollider;

	public string navigationMode = "Spatial";
	public string selectionMode = "Point";

	public static BoxCollider layerMoveBaseCollider;
	
	void Start () {
		UpdateNavigationMode ();
		UpdateSelectionMode ();
	}

	void UpdateNavigationMode(){
		if (navigationMode == "Lever") {
			layerMoveBaseCollider = leverMovedObjectCollider;
		} else if (navigationMode == "Spatial") {
			layerMoveBaseCollider = cameraMainCollider;
		} else {
			Debug.LogError ("Unknown navigation mode: "+navigationMode, gameObject);
		}
	}

	void UpdateSelectionMode(){
		if (selectionMode == "Point") {
			// TODO : Gaze/Point selection mode switch
		} else if (selectionMode == "Gaze") {
			// TODO : Gaze/Point selection mode switch
		} else {
			Debug.LogError ("Unknown selection mode: "+selectionMode, gameObject);
		}
	}
}