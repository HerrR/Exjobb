using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShadowCanvas : MonoBehaviour {
	GameObject mainCanvas;

	void Awake(){
		// CreateFrontShadowCanvas ();
	}

	public void PositionCanvasToMainCanvas(){
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		gameObject.transform.position = mainCanvas.transform.position;
	}

	void CreateFrontShadowCanvas(){
		GameObject newShadowCanvas = GameObject.Instantiate (
			gameObject,
			transform.position,
			transform.rotation,
			transform.parent
		);

		Destroy (newShadowCanvas.GetComponent<ShadowCanvas> ());
		newShadowCanvas.AddComponent<FrontShadowCanvas> ();
		newShadowCanvas.name = "Front ShadowCanvas";
		newShadowCanvas.tag = "ShadowCanvasFront";
	}

	void UpdateFrontShadowCanvas(){
		
	}
}
