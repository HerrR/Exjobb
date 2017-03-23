using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
	public BoxCollider cameraMainCollider;
	public BoxCollider leverMovedObjectCollider;

	public string _navigationMode = "Spatial";
	public string _selectionMode = "Gaze";

	public static string navigationMode;
	public static string selectionMode;

	public static BoxCollider layerMoveBaseCollider;

	public GameObject[] referenceImages;
	
	void Awake () {
		UpdateNavigationMode ();
		UpdateSelectionMode ();
	}

	void Update(){
		CheckKeypress ();
	}

	void CheckKeypress(){
		bool keyPressed = false;
		int keyNum = -1;

		if (Input.GetKeyDown (KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) {
			keyNum = 0;
			keyPressed = true;
		}

		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
			keyNum = 1;
			keyPressed = true;
		}

		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
			keyNum = 2;
			keyPressed = true;
		}

		if (!keyPressed) {
			return;
		}

		if (keyNum == 0) {
			HideAllReferenceImages ();
		} else if (keyNum <= referenceImages.Length) {
			HideAllReferenceImages ();
			referenceImages [keyNum - 1].SetActive (true);
			// Debug.Log (referenceImages [keyNum - 1], referenceImages [keyNum - 1]);
		}
	}

	void HideAllReferenceImages(){
		foreach (GameObject refImg in referenceImages) {
			refImg.SetActive (false);
		}
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