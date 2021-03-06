﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CompositeImage : MonoBehaviour {
	public GameObject layerPrefab;
	public Image canvasFrame;
	public Zone layerGenerationZone;
	private LayerManager layerManager;

	void Start() {
		FindCanvasFrame ();
		FindGenerationZone ();
		PositionCanvasToGenerationZone ();
		layerManager = GameObject.FindObjectOfType<LayerManager>();
		GenerateLayers (layerGenerationZone.bounds.zMin + 0.01f, layerGenerationZone.bounds.zMax - 0.01f);
		HideFrame ();
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

	void FindGenerationZone() {
		#pragma warning disable 0168
		try {
			layerGenerationZone = GameObject.FindGameObjectWithTag("GenerationZone").GetComponent<Zone>();
		} catch (Exception e) {
		
			Debug.LogError ("Failed to find generation zone");
		}
		#pragma warning restore 0168
	}

	void PositionCanvasToGenerationZone() {
		Vector3 currentPosition = gameObject.transform.position;
		Vector3 newPosition = new Vector3 (
			currentPosition.x, 
			currentPosition.y, 
			layerGenerationZone.bounds.zMax
		);
		gameObject.transform.position = newPosition;
	}

	Image[] GetRelevantImages(){
		List<Image> relevantLayerList = new List<Image> (gameObject.GetComponentsInChildren<Image> ());

		for (int i = relevantLayerList.Count - 1 ; i >= 0; i--) {
			if (relevantLayerList [i].gameObject.tag == "MainCanvas") {
				relevantLayerList.RemoveAt (i);
				continue;
			}

			if (relevantLayerList [i].gameObject.tag == "CanvasFrame") {
				relevantLayerList.RemoveAt (i);
				continue;
			}

			if (relevantLayerList [i].gameObject.tag == "Crosshair") {
				relevantLayerList.RemoveAt (i);
				continue;
			}
		}

		Image[] relevantLayers = relevantLayerList.ToArray ();

		return relevantLayers;
	}

	public void GenerateLayers(float zMin, float zMax){
		Image[] layers = GetRelevantImages ();

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
			// newLayer.GetComponentInChildren<Text> ().text = "Layer " + layersGenerated;
			newLayer.GetComponent<Layer> ().basePosition = spawnPosition;

			Image imageCopy = Instantiate (
				layerImage,
				new Vector3(layerImage.transform.position.x, layerImage.transform.position.y, newLayer.transform.position.z),
				spawnRotation,
				newLayer.transform
			);

			Destroy (layerImage.GetComponent<LayerImage> ());
			Destroy (layerImage.GetComponent<ImageSwitcher> ());
			layerImage.gameObject.AddComponent<ShadowImage> ();
			layerImage.GetComponent<ShadowImage> ().SetTrackedImage (imageCopy);
			layerImage.gameObject.name = newLayer.name + " shadow";
			layerImage.gameObject.tag = "ShadowImage";
			layersGenerated++;
		}
		layerManager.FindLayers ();
	}

	public void HideFrame(){
		canvasFrame.gameObject.SetActive (false);
	}

	public void ShowFrame(){
		canvasFrame.gameObject.SetActive (true);
	}
}