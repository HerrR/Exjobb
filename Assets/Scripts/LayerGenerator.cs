using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerGenerator : MonoBehaviour {
	public Image[] layerImages;
	public GameObject layerPrefab;

	private float distanceBetweenLayers = 0.3f;
	private float distanceFromFirstLayer = 0.3f;

	public void GenerateLayers(){
		Debug.Log ("Generate layers");
	}
}
