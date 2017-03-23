using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShadowCanvas : MonoBehaviour {
	GameObject mainCanvas;
	GameObject shadowCanvasFront;
	Zone generationZone;

	void Awake(){
		generationZone = GameObject.FindGameObjectWithTag ("GenerationZone").GetComponent<Zone> ();
	}

	public void PositionCanvasToMainCanvas(){
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		gameObject.transform.position = mainCanvas.transform.position;
	}

	public void AdjustToGenerationZone(){
		Vector3 newPos = gameObject.transform.position;

		if(gameObject.CompareTag("ShadowCanvas")){
			newPos.z = generationZone.bounds.zMax;	
		}

		if (gameObject.CompareTag ("ShadowCanvasFront")) {
			newPos.z = generationZone.bounds.zMin;
		}

		gameObject.transform.position = newPos;
	}

	void UpdateFrontShadowCanvas(){
		
	}
}
