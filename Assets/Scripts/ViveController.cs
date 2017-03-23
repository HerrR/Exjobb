using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	public ControllerSelectionManager controllerSelectionManager;
	private GazeSelectionManager gazeSelectionManager;

	private float triggerLastPressed;
	private float triggerDownToHoldTime;
	private float triggerDownToHoldTimeDefault = 0.5f;

	private Vector3 lastTrackpadPosition;

	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		controllerSelectionManager = gameObject.GetComponentInChildren<ControllerSelectionManager> ();
		gazeSelectionManager = GameObject.FindObjectOfType<GazeSelectionManager> ();
		triggerDownToHoldTime = triggerDownToHoldTimeDefault;
	}

	void Update () {
		device = SteamVR_Controller.Input ((int)trackedObject.index);

		// Trackpad
		if (device.GetAxis ().x != 0 || device.GetAxis ().y != 0) {
			controllerSelectionManager.OnViveControllerTrackpad (new Vector2 (device.GetAxis ().x, device.GetAxis ().y));
			gazeSelectionManager.OnViveControllerTrackpad (new Vector2 (device.GetAxis ().x, device.GetAxis ().y));
		}

		// Trackpad pressed
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			lastTrackpadPosition = gameObject.transform.position;
		}

		// Trackpad continuous press
		if (device.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector3 diffVector = gameObject.transform.position - lastTrackpadPosition;
			controllerSelectionManager.OnViveControllerTrackpadPress (diffVector);
			lastTrackpadPosition = gameObject.transform.position;
		}

		// Trackpad release
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {

		}

		// Trigger - Trigger on first click
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			triggerLastPressed = Time.time;
			controllerSelectionManager.OnViveControllerTrigger ();
			gazeSelectionManager.OnViveControllerTrigger ();
		}

		// Trigger - Contiuous on trigger down
		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			if (Time.time - triggerLastPressed > triggerDownToHoldTime) {
				controllerSelectionManager.OnViveControllerTriggerHold (gameObject.transform.position);
				gazeSelectionManager.OnViveControllerTriggerHold (gameObject.transform.position);
			} 
		}

		// Trigger - Released
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			triggerLastPressed = default(float);

			controllerSelectionManager.OnViveControllerTriggerRelease ();
			gazeSelectionManager.OnViveControllerTriggerRelease ();
		}

		// Application menu
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
			controllerSelectionManager.OnViveControllerApplicationMenu ();
			gazeSelectionManager.OnViveControllerApplicationMenu();
		}

		// Grip
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			controllerSelectionManager.OnViveControllerGrip ();
			gazeSelectionManager.OnViveControllerGrip ();
		}
	}

	public void Vibrate(ushort force) {
		device.TriggerHapticPulse (force);
	}

	public void SetTriggerDownHoldTime(float _holdTime){
		triggerDownToHoldTime = _holdTime;
	}

	public void ResetTriggerDownHoldTime(){
		triggerDownToHoldTime = triggerDownToHoldTimeDefault;
	}
}
