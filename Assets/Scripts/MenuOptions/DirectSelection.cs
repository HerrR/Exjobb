using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectSelection : MenuOption {
	Settings settings;
	// Use this for initialization
	void Start () {
		OnStart ();
		settings = GameObject.FindObjectOfType<Settings> ();
	}
	
	// Update is called once per frame
	void Update () {
		OnUpdate ();
		UpdateActive ();
	}

	void UpdateActive(){
		selected = (Settings.selectionMode == "Direct");
	}

	public override void SelectOption(){
		settings.SetSelectionMode (1);
	}
}
