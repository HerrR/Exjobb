using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Layer : MonoBehaviour {
	public GameObject mainCanvas;
	public GameObject mainCamera;
	public GameObject inspectionZone;
	public Vector3 basePosition;
	public bool accordion;

	private float zMax;
	private float zMin;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		inspectionZone = GameObject.FindGameObjectWithTag ("InspectionZone");
		zMax = inspectionZone.GetComponent<Zone> ().bounds.zMax;
		zMin = inspectionZone.GetComponent<Zone> ().bounds.zMin;
		accordion = true;
	}

	void Update(){
		if (accordion) {
			AccordionMove ();
		}
	}

	void AccordionMove(){
		float OffsetPos = Mathf.Pow ((basePosition.z - mainCamera.transform.position.z), 2);
		Vector3 _pos = gameObject.transform.position;
		_pos.z = (basePosition.z + OffsetPos);
		if (_pos.z > zMax) {
			_pos.z = zMax;
		}

		if (_pos.z < zMin) {
			_pos.z = zMin;
		}

		if (basePosition.z < mainCamera.transform.position.z) {
			_pos.z = basePosition.z;
		}

		Debug.Log ("Pos: " + _pos.z + " ,Min: " + zMin + " ,Max: " + zMax , gameObject);
		gameObject.transform.position = _pos;
	}

	public void copyToCanvas(){
		Quaternion canvasRotation = mainCanvas.transform.rotation;
		Image layerImage = gameObject.GetComponentInChildren<Image> ();
		Vector3 spawnPosition = layerImage.transform.position;
		spawnPosition.z = mainCanvas.transform.position.z;

		Image imageCopy = Instantiate (
			layerImage,
			spawnPosition,
			canvasRotation,
			mainCanvas.transform
		);

		imageCopy.name = "Image";
	}


}
