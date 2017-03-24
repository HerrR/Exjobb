using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.IO;

public class Settings : MonoBehaviour {
	public BoxCollider cameraMainCollider;
	public BoxCollider leverMovedObjectCollider;

	public string _navigationMode = "Spatial";
	public string _selectionMode = "Gaze";

	public static string navigationMode;
	public static string selectionMode;

	public static BoxCollider layerMoveBaseCollider;

	public GameObject[] referenceImages;

	public float taskStartTime;

	public string fileName = "UserTestLog.txt";
	public Logger logger;
	
	void Awake () {
		UpdateNavigationMode ();
		UpdateSelectionMode ();
	}

	void Start(){
		logger = GameObject.FindObjectOfType<Logger> ();
		logger.SetFileName (fileName);
	}


	void Update(){
		CheckReferenceImgageKeypress ();
		CheckLogKeypress ();
	}

	void LogNewParticipant(){
		string[] introMessage = {
			"------------------------------------------------------",
			"New Participant",
			"Session started " + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"),
			"Selection mode: " + selectionMode,
			"Navigation mode: " + navigationMode,
			"------------------------------------------------------"
		};
		Logger.WriteToLog(introMessage);
	}

	void StartTask(){
		taskStartTime = Time.time;
		string[] msg = {
			"###",
			"Task started: " + taskStartTime,
			"Reference image: " + ReferenceImageBeingShown()
		};
		Logger.WriteToLog (msg);
		// WriteToLog (msg);
	}

	void FinishTask(){
		string[] msg = {
			"Task finished: " + (Time.time),
			"Time to complete: " + (Time.time - taskStartTime),
			"###"
		};
		Logger.WriteToLog (msg);
		taskStartTime = default(float);
	}

	void LogSessionComplete(){
		string[] msg = {
			"###",
			"Session finished " + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"),
			"###"
		};

		Logger.WriteToLog (msg);
	}

	void CheckLogKeypress(){
		if (Input.GetKeyDown (KeyCode.N)) {
			LogNewParticipant ();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			StartTask ();
		}

		if (Input.GetKeyDown (KeyCode.F)) {
			FinishTask ();
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			LogSessionComplete ();
		}
	}

	string ReferenceImageBeingShown(){
		for (int i = 0; i < referenceImages.Length; i++) {
			if (referenceImages [i].activeSelf) {
				return referenceImages [i].name;
			}
		}

		return "None";
	}

	void CheckReferenceImgageKeypress(){
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