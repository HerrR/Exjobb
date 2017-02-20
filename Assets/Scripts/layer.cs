using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Layer : MonoBehaviour {
	public GameObject mainCanvas;
	public Vector3 basePosition;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
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
