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

	public string _navigationMode = "Spatial";
	public string _selectionMode = "Gaze";

	public static string navigationMode;
	public static string selectionMode;

	public static BoxCollider layerMoveBaseCollider;
	
	void Awake () {
		UpdateNavigationMode ();
		UpdateSelectionMode ();
	}

	void UpdateNavigationMode(){

		if (_navigationMode == "Lever") {
			navigationMode = _navigationMode;
			layerMoveBaseCollider = leverMovedObjectCollider;
		} else if (_navigationMode == "Spatial") {
			navigationMode = _navigationMode;
			layerMoveBaseCollider = cameraMainCollider;
		} else {
			Debug.LogError ("Unknown navigation mode: "+_navigationMode, gameObject);
		}
	}

	void UpdateSelectionMode(){
		if (_selectionMode == "Point") {
			// TODO : Gaze/Point selection mode switch
			selectionMode = _selectionMode;
		} else if (_selectionMode == "Gaze") {
			// TODO : Gaze/Point selection mode switch
			selectionMode = _selectionMode;
		} else {
			Debug.LogError ("Unknown selection mode: "+_selectionMode, gameObject);
		}
	}
}