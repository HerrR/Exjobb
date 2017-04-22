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

	public static string navigationMode;
	public static string selectionMode;

	public static BoxCollider layerMoveBaseCollider;

	public GameObject[] referenceImages;

	public float taskStartTime;

	public string fileName = "UserTestLog.txt";
	public Logger logger;

	public string[] selectionModes = {"Pointer", "Direct"};
	public int activeSelectionMode;
	
	void Awake () {
		activeSelectionMode = 0;
		SetSelectionMode(activeSelectionMode);
	}

	void Start(){
		logger = GameObject.FindObjectOfType<Logger> ();
		logger.SetFileName (fileName);
	}

	void Update(){
		CheckReferenceImgageKeypress ();
		CheckKeypress ();
	}

	public void NextSelectionMode(){
		int numModes = selectionModes.Length;
		int nextSelectionMode = activeSelectionMode + 1;
		if (nextSelectionMode > numModes - 1) {
			nextSelectionMode = 0;
		}
		SetSelectionMode (nextSelectionMode);
	}

	void CheckKeypress(){
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

		if (Input.GetKeyDown (KeyCode.M)) {
			NextSelectionMode ();
		}
	}

	/*
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
	*/

	void SetSelectionMode(int modeIndex){
		try {
			selectionMode = selectionModes [modeIndex];
			activeSelectionMode = modeIndex;
		} catch {
			Debug.LogError ("Failed to update selection mode with index "+ modeIndex, gameObject);
		}
	}


								/******************
									  LOGGING
								******************/


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
}