using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	private ControllerSelectionManager controllerSelectionManager;

	// Use this for initialization
	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		controllerSelectionManager = gameObject.GetComponentInChildren<ControllerSelectionManager> ();
	}
		
	public void Vibrate(ushort force) {
		device.TriggerHapticPulse (force);
	}

	// Update is called once per frame
	void Update () {
		device = SteamVR_Controller.Input ((int)trackedObject.index);

		// Trackpad
		if (device.GetAxis ().x != 0 || device.GetAxis ().y != 0) {
			Debug.Log (device.GetAxis ().x + " " + device.GetAxis ().y);
		}

		// Trigger
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			device.TriggerHapticPulse (3500);
			controllerSelectionManager.OnViveControllerTrigger ();
		}
	}
}
