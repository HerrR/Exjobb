using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	private ControllerSelectionManager controllerSelectionManager;

	private float triggerLastPressed;
	private float triggerDownToHoldTime;

	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		controllerSelectionManager = gameObject.GetComponentInChildren<ControllerSelectionManager> ();
		triggerDownToHoldTime = 1.5f;
	}

	void Update () {
		device = SteamVR_Controller.Input ((int)trackedObject.index);

		// Trackpad
		if (device.GetAxis ().x != 0 || device.GetAxis ().y != 0) {
			controllerSelectionManager.OnViveControllerTrackpad (new Vector2 (device.GetAxis ().x, device.GetAxis ().y));
		}

		// Trigger - Trigger on first click
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			triggerLastPressed = Time.time;
			controllerSelectionManager.OnViveControllerTrigger ();
		}

		// Trigger - Contiuous on trigger down
		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			if (Time.time - triggerLastPressed > triggerDownToHoldTime) {
				controllerSelectionManager.OnViveControllerTriggerHold (gameObject.transform.position);
			} 
		}

		// Trigger - Released
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			triggerLastPressed = default(float);
			controllerSelectionManager.OnViveControllerTriggerRelease ();
		}

		// Application menu
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
			controllerSelectionManager.OnViveControllerApplicationMenu ();
		}

		// Grip
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			controllerSelectionManager.OnViveControllerGrip ();
		}
	}

	public void Vibrate(ushort force) {
		device.TriggerHapticPulse (force);
	}
}
