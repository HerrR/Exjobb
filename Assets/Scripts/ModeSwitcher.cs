using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class mode_switcher : MonoBehaviour {
	public GameObject textUpdateOnChange;
	public string mode;

	public GameObject mainCanvas;
	public GameObject[] layers;
	public Image[] imageLayers;
	public GameObject layerPrefab;

	void Start(){
		// mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		// mainCanvasLayerManager = mainCanvas.GetComponent<LayerManager> ();
		// mainCanvasLayerManager.canvasToLayers ();




		/*
		imageLayers = mainCanvas.GetComponentsInChildren<Image> ();
		generateLayersFromMainCanvas ();
		*/ 
		// layers = GameObject.FindGameObjectsWithTag ("Layer");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Zone> ()) {
			switchMode (other.gameObject.GetComponent<Zone>().name);
			if (textUpdateOnChange) {
				textUpdateOnChange.GetComponent<Text> ().text = other.gameObject.GetComponent<Zone>().name;
			} 
		}
	}
	/* 
	void generateLayersFromMainCanvas(){
		int layersGenerated = 0;
		foreach (Image image in imageLayers) {

			Vector3 spawnPosition = mainCanvas.transform.parent.gameObject.transform.position;
			spawnPosition.z += (-layersGenerated*distanceFromFirstLayer - distanceBetweenLayers);

			Quaternion spawnRotation = mainCanvas.transform.parent.gameObject.transform.rotation;
			
			GameObject newLayer = Instantiate(
				layerPrefab, 
				spawnPosition, 
				spawnRotation, 
				mainCanvas.transform.parent.gameObject.transform);

			Image newLayerImage = Instantiate (
				image,
				new Vector3(image.transform.position.x, image.transform.position.y, newLayer.transform.position.z),
				image.transform.rotation,
				newLayer.transform);

			Text layerText = newLayer.GetComponentInChildren<Text> ();
			layerText.text = "Layer " + layersGenerated;

			layersGenerated++;
		}
	}
	*/ 

	void switchMode(string _mode){
		mode = _mode;

		switch (mode){
		case "overview":
			/* 
			mainCanvas.gameObject.SetActive (true);
			foreach (GameObject layer in layers) {
				layer.gameObject.SetActive (false);
			}
			*/
			break;
		case "inspection":
			/* 
			mainCanvas.gameObject.SetActive(false);
			foreach (GameObject layer in layers) {
				layer.gameObject.SetActive (true);
			}
			*/
			break;
		default:
			break;
		}

	}
}
