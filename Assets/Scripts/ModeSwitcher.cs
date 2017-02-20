using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour {
	public string mode;

	// Assigned in inspector
	public GameObject textUpdateOnChange;

	// Auto assigned by tag
	public GameObject mainCanvas;

	// Auto assigned by tag
	public GameObject inspectionZone;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		inspectionZone = GameObject.FindGameObjectWithTag ("InspectionZone");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Zone> ()) {
			switchMode (other.gameObject.GetComponent<Zone>().name);
			if (textUpdateOnChange) {
				textUpdateOnChange.GetComponent<Text> ().text = other.gameObject.GetComponent<Zone>().name;
			} 
		}
	}

	void switchMode(string _mode){
		mode = _mode;

		switch (mode){
		case "overview":
			foreach(GameObject layer in GameObject.FindGameObjectsWithTag("Layer")){
				layer.GetComponent<Layer> ().copyToCanvas ();
				Destroy (layer);
			}
			break;
		case "inspection":
			float zMin = inspectionZone.GetComponent<Zone> ().bounds.zMin;
			float zMax = inspectionZone.GetComponent<Zone> ().bounds.zMax;
			mainCanvas.GetComponent<CompositeImage> ().GenerateLayers (zMin, zMax);
			mainCanvas.GetComponent<CompositeImage> ().ClearCanvas ();
			break;
		default:
			break;
		}

	}
}


/*
public class ModeSwticher : MonoBehaviour {
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




		
		imageLayers = mainCanvas.GetComponentsInChildren<Image> ();
		generateLayersFromMainCanvas ();

		// layers = GameObject.FindGameObjectsWithTag ("Layer");
	}


	
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


}
*/