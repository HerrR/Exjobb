using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapse : MenuOption {
	private LayerManager layerManager;

	// Use this for initialization
	void Start () {
		OnStart ();
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		OnUpdate ();
		UpdateShowStatus ();
	}

	void UpdateShowStatus(){
		// Show this option if the layers are expanded
		showWhenMenuActive = layerManager.rearrangementMode;
	}

	public override void SelectOption(){
		layerManager.CollapseLayers ();
		Hide ();
	}
}
