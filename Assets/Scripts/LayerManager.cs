using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerManager : MonoBehaviour {
	public Image[] layerImages;
	public GameObject layerPrefab;

	private float distanceBetweenLayers = 0.3f;
	private float distanceFromFirstLayer = 0.3f;

	public void canvasToLayers(){
		Debug.Log ("Canvas to layers called");
	}
	
	void layersToCanvas(Canvas targetCanvas){
		Debug.Log ("Layers to canvas called");
	}
}







/*
	void generateLayers(){
		Debug.Log ("Generate layers called");
		layerImages = gameObject.GetComponentsInChildren<Image> ();
		int layersGenerated = 0;
		foreach (Image image in layerImages) {
			Vector3 spawnPosition = gameObject.transform.parent.gameObject.transform.position;
			spawnPosition.z += (-layersGenerated*distanceFromFirstLayer - distanceBetweenLayers);

			Quaternion spawnRotation = gameObject.transform.parent.gameObject.transform.rotation;

			GameObject newLayer = Instantiate(
				layerPrefab, 
				spawnPosition, 
				spawnRotation, 
				gameObject.transform.parent.gameObject.transform);

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
