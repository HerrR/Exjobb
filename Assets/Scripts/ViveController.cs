using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	private TriggerDownProgress triggerDownProgress;
	private ControllerSelectionManager controllerSelectionManager;
	private Pointer pointer;
	// private GazeSelectionManager gazeSelectionManager;

	private float triggerLastPressed;
	private float triggerDownToHoldTime;
	private float triggerDownToHoldTimeDefault = 0.5f;
	private float showTriggerHoldThreshold = 0.2f;

	// private Vector3 lastTrackpadPosition;
	public bool triggerHold;
	public bool gripDown;
	public bool trackpadDown;

	private bool triggerHoldLogged = false;

	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		controllerSelectionManager = gameObject.GetComponentInChildren<ControllerSelectionManager> ();
		pointer = gameObject.GetComponent<Pointer> ();
		triggerDownProgress = GetComponentInChildren<TriggerDownProgress> ();
		triggerDownProgress.Hide ();
		triggerDownToHoldTime = triggerDownToHoldTimeDefault;
	}

	void Update () {
		device = SteamVR_Controller.Input ((int)trackedObject.index);

		// Trackpad
		if (device.GetAxis ().x != 0 || device.GetAxis ().y != 0) {
			// controllerSelectionManager.OnViveControllerTrackpad (new Vector2 (device.GetAxis ().x, device.GetAxis ().y));
			// gazeSelectionManager.OnViveControllerTrackpad (new Vector2 (device.GetAxis ().x, device.GetAxis ().y));
		}

		// Trackpad pressed
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			trackpadDown = true;
			Logger.LogKeypress ("Trackpad Down");
			if (Settings.selectionMode == "Pointer") {
				pointer.OnControllerTrackpad ();
			}
			if (Settings.selectionMode == "Direct") {
				controllerSelectionManager.OnViveControllerTrackpad ();
			}
			// lastTrackpadPosition = gameObject.transform.position;
		}

		// Trackpad continuous press
		if (device.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			if (Settings.selectionMode == "Pointer") {
				pointer.OnControllerContinuousTrackpad ();
			}

			if (Settings.selectionMode == "Direct") {
				controllerSelectionManager.OnViveControllerTrackpadContinuous ();
			}
		}

		// Trackpad release
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
			Logger.LogKeypress ("Trackpad Up");
			trackpadDown = false;
			if (Settings.selectionMode == "Pointer") {
				pointer.OnControllerTrackpadRelease ();
			}

			if (Settings.selectionMode == "Direct") {
				controllerSelectionManager.OnViveControllerTrackpadRelease ();
			}
		}

		// Trigger - Trigger on first click
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			Logger.LogKeypress ("Trigger Down");
			triggerLastPressed = Time.time;
			if (Settings.selectionMode == "Pointer") {
				pointer.OnControllerTrigger ();
			}

			if (Settings.selectionMode == "Direct") {
				controllerSelectionManager.OnViveControllerTrigger ();
			}
		}

		// Trigger - Contiuous on trigger down
		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			triggerHold = (Time.time - triggerLastPressed > triggerDownToHoldTime);

			if (!triggerHold) {
				float progress = (Time.time - triggerLastPressed - showTriggerHoldThreshold) / (triggerDownToHoldTime - showTriggerHoldThreshold);
				if (progress > 0) {
					triggerDownProgress.Show ();
				}
				triggerDownProgress.SetProgress(progress);
				return;
			}
			triggerDownProgress.SetProgress (1);

			if (!triggerHoldLogged) {
				Logger.LogKeypress ("Trigger Hold");
				triggerHoldLogged = true;
			}

			if (Settings.selectionMode == "Pointer") {
				pointer.OnControllerTriggerHold ();
			}

			if (Settings.selectionMode == "Direct") {
				controllerSelectionManager.OnViveControllerTriggerHold ();
			}
		}

		// Trigger - Released
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			Logger.LogKeypress ("Trigger Up");
			triggerHoldLogged = false;

			triggerLastPressed = default(float);
			triggerDownProgress.Hide ();
			triggerHold = false;

			if(Settings.selectionMode == "Pointer"){
				pointer.OnControllerTriggerRelease ();
				return;
			}

			if (Settings.selectionMode == "Direct") {
				controllerSelectionManager.OnViveControllerTriggerRelease ();
				return;
			}


		}

		// Application menu
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
		}

		// Grip
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			Logger.LogKeypress ("Grip Down");
			gripDown = true;
		}

		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Grip)) {
			Logger.LogKeypress ("Grip Up");
			gripDown = false;	
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
