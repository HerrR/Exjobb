using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerOffhand : MonoBehaviour {
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	private Menu menu;

	// Use this for initialization
	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		menu = GameObject.FindObjectOfType<Menu> ();
	}
	
	// Update is called once per frame
	void Update () {
		device = SteamVR_Controller.Input ((int)trackedObject.index);

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			menu.ToggleMenu ();
		}
	}
}
