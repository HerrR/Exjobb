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

	private int distanceBetweenLayers = 5;
	private int distanceFromFirstLayer = 5;

	void Start(){
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		imageLayers = mainCanvas.GetComponentsInChildren<Image> ();
		generateLayersFromMainCanvas ();
		layers = GameObject.FindGameObjectsWithTag ("Layer");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<zone> ()) {
			switchMode (other.gameObject.GetComponent<zone>().name);
			if (textUpdateOnChange) {
				textUpdateOnChange.GetComponent<Text> ().text = other.gameObject.GetComponent<zone>().name;
			} 
		}
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
			
			Image layerImage = newLayer.GetComponentInChildren<Image> ();
			layerImage.sprite = image.GetComponent<Image>().sprite;

			Text layerText = newLayer.GetComponentInChildren<Text> ();
			layerText.text = "Layer " + layersGenerated;
			layersGenerated++;
		}
	}

	void switchMode(string _mode){
		mode = _mode;

		switch (mode){
		case "overview":
			Debug.Log ("Switching to overview mode");
			mainCanvas.gameObject.SetActive (true);
			foreach (GameObject layer in layers) {
				layer.gameObject.SetActive (false);
			}
			break;
		case "inspection":
			Debug.Log ("Switching to inspection mode");
			mainCanvas.gameObject.SetActive(false);
			foreach (GameObject layer in layers) {
				layer.gameObject.SetActive (true);
			}
			break;
		default:
			Debug.LogError ("Default switch case, invalid mode");
			break;
		}

	}
}
