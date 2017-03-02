﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CompositeImage : MonoBehaviour {
	public GameObject layerPrefab;
	public Image canvasFrame;

	void Start() {
		FindCanvasFrame ();
	}

	void FindCanvasFrame() {
		#pragma warning disable 0168
		try {
			canvasFrame = GameObject.FindGameObjectWithTag ("CanvasFrame").GetComponent<Image>();
		} catch (Exception e) {
			Debug.LogError ("Could not find canvas frame");
		}
		#pragma warning restore 0168
	}

	Image[] GetLayersExcludingCanvasAndFrame(){
		List<Image> relevantLayerList = new List<Image> (gameObject.GetComponentsInChildren<Image> ());

		for (int i = relevantLayerList.Count - 1 ; i >= 0; i--) {
			if (relevantLayerList [i].gameObject.tag == "MainCanvas") 
				relevantLayerList.RemoveAt (i);

			if (relevantLayerList [i].gameObject.tag == "CanvasFrame") 
				relevantLayerList.RemoveAt (i);
		}

		Image[] relevantLayers = relevantLayerList.ToArray ();

		return relevantLayers;
	}

	public void GenerateLayers(float zMin, float zMax){
//		Debug.Log (layers.OrderByDescending (img => img.gameObject.GetComponent<Layer> ().basePosition.z).ToArray ());
//		layers = GameObject.FindGameObjectsWithTag ("Layer").OrderByDescending (go => go.transform.position.z).ToArray ();
//		layers = layers.OrderByDescending (img => img.gameObject.GetComponent<Layer> ().basePosition.z).ToArray ();

		Image[] layers = GetLayersExcludingCanvasAndFrame ();

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

			newLayer.name = "Layer "+layersGenerated;
			newLayer.GetComponentInChildren<Text> ().text = "Layer " + layersGenerated;
			newLayer.GetComponent<Layer> ().basePosition = spawnPosition;

			Image newLayerImage = Instantiate (
				layerImage,
				new Vector3(layerImage.transform.position.x, layerImage.transform.position.y, newLayer.transform.position.z),
				spawnRotation,
				newLayer.transform
			);
				
			layersGenerated++;
		}
	}

	public void ClearCanvas(){
		Image[] layers = GetLayersExcludingCanvasAndFrame ();
		foreach (Image layer in layers) {
			Destroy (layer.gameObject);
		}
	}

	public void HideFrame(){
		canvasFrame.gameObject.SetActive (false);
		// GameObject frame = GameObject.FindGameObjectWithTag ("CanvasFrame");
		// frame.SetActive (false);
	}

	public void ShowFrame(){
		canvasFrame.gameObject.SetActive (true);
		// GameObject frame = GameObject.FindGameObjectWithTag ("CanvasFrame");
		// frame.SetActive (true);
	}
}