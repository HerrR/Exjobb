using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CompositeImage : MonoBehaviour {
	public GameObject layerPrefab;

	public void GenerateLayers(float zMin, float zMax){
		Image[] layers = gameObject.GetComponentsInChildren<Image> ();
		int layersGenerated = 0;
		float distanceBetweenLayers = (zMax - zMin) / layers.Length;

		foreach (Image layerImage in layers) {
			Vector3 spawnPosition = gameObject.transform.position;
			spawnPosition.z = zMax;
			spawnPosition.z -= (layersGenerated * distanceBetweenLayers);
			Quaternion spawnRotation = gameObject.transform.parent.gameObject.transform.rotation;

			GameObject newLayer = Instantiate (
				layerPrefab,
				spawnPosition,
				spawnRotation,
				gameObject.transform.parent.gameObject.transform
			);

			Image newLayerImage = Instantiate (
				layerImage,
				new Vector3(layerImage.transform.position.x, layerImage.transform.position.y, newLayer.transform.position.z),
				spawnRotation,
				newLayer.transform
			);

			newLayer.name = "Layer "+layersGenerated;
			newLayer.GetComponentInChildren<Text> ().text = "Layer " + layersGenerated;;
			newLayer.GetComponent<Layer> ().basePosition = spawnPosition;

			layersGenerated++;
		}
	}

	public void ClearCanvas(){
		foreach (Image layer in gameObject.GetComponentsInChildren<Image> ()) {
			Destroy (layer.gameObject);
		}
	}

	public void HideFrame(){
		GameObject frame = gameObject.transform.FindChild ("Frame").gameObject;
		frame.SetActive (false);
	}

	public void ShowFrame(){
		GameObject frame = gameObject.transform.FindChild ("Frame").gameObject;
		frame.SetActive (true);
	}
}