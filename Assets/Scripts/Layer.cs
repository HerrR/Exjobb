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
	public Image layerImage;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		inspectionZone = GameObject.FindGameObjectWithTag ("InspectionZone");
		zMax = inspectionZone.GetComponent<Zone> ().bounds.zMax;
		zMin = inspectionZone.GetComponent<Zone> ().bounds.zMin;
		accordion = true;
		FindLayerImage ();
	}

	void Update(){
		if (accordion) {
			AccordionMove ();
		}
	}

	// TODO: Fix this shit
	public bool isSelected() {
		return true;
	}

	void FindLayerImage(){
		Image[] layerImages = gameObject.GetComponentsInChildren<Image> ();

		foreach (Image img in layerImages) {
			if (img.gameObject.tag != "DontCopy") {
				layerImage = img;
			}
		}
	}

	void AccordionMove(){
		float offsetPos = 1000 * Mathf.Pow (
			(basePosition.z - mainCamera.transform.position.z) - 0.3f
			, 7);
		Vector3 _pos = gameObject.transform.position;
		_pos.z = Mathf.Clamp ((basePosition.z + offsetPos), zMin, zMax);

		gameObject.transform.position = _pos;
	}

	public void copyToCanvas(){
		Quaternion canvasRotation = mainCanvas.transform.rotation;
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
