using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShadowCanvas : MonoBehaviour {
	Zone layerGenerationZone;
	GameObject mainCanvas;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		FindGenerationZone ();
		PositionCanvasToMainCanvas ();
	}

	void FindGenerationZone() {
		#pragma warning disable 0168
		try {
			layerGenerationZone = GameObject.FindGameObjectWithTag("GenerationZone").GetComponent<Zone>();
		} catch (Exception e) {

			Debug.LogError ("Failed to find generation zone");
		}
		#pragma warning restore 0168
	}

	void PositionCanvasToGenerationZone() {
		Vector3 currentPosition = gameObject.transform.position;
		Vector3 newPosition = new Vector3 (
			currentPosition.x, 
			currentPosition.y, 
			layerGenerationZone.bounds.zMax
		);
		gameObject.transform.position = newPosition;
	}

	void PositionCanvasToMainCanvas(){
		gameObject.transform.position = mainCanvas.transform.position;
	}
}
