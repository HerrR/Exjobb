using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	private MenuOption[] menuOptions;
	private bool menuActive;
	public ViveControllerOffhand controller;
	public GameObject mainCamera;
	public Pointer pointer;
	public ControllerSelectionManager controllerSelectionManager;

	void Start () {
		menuOptions = GameObject.FindObjectsOfType<MenuOption>();
		menuActive = false;
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Update () {
		if (controller == default(ViveControllerOffhand)) {
			FindController ();
		}

		if (IsActive ()) {
			UpdateHovered ();
		}
	}

	void UpdateHovered(){
		if (Settings.selectionMode == "Direct") {
			foreach (MenuOption menuOption in menuOptions) {
				if (GameObject.Equals (controllerSelectionManager.currentTarget.target, menuOption.gameObject)) {
					menuOption.SetHovered (true);
				} else {
					menuOption.SetHovered (false);
				}
			}
		}

		if (Settings.selectionMode == "Pointer") {
			foreach (MenuOption menuOption in menuOptions) {
				if (GameObject.Equals (pointer.currentTarget.target, menuOption.gameObject)) {
					menuOption.SetHovered (true);
				} else {
					menuOption.SetHovered (false);
				}
			}
		}
	}

	void FindController(){
		controller = GameObject.FindObjectOfType<ViveControllerOffhand> ();
	}

	void MoveToController(){
		Vector3 pos = controller.transform.position;
		pos.y = pos.y + 0.2f;
		gameObject.transform.position = pos;
	}

	void FaceUser(){
		gameObject.transform.LookAt (mainCamera.transform.position);
	}

	public void ToggleMenu(){
		if (menuActive) {
			HideMenu ();
		} else {
			ShowMenu ();
		}
	}

	public void ShowMenu(){
		MoveToController ();
		FaceUser ();
		menuActive = true;
	}

	public void HideMenu(){
		if (Settings.selectionMode == "Direct") {
			controllerSelectionManager.ResetTargets ();
		}
		menuActive = false;
	}

	public void ResetTargets(){
		if (Settings.selectionMode == "Direct") {
			controllerSelectionManager.ResetTargets ();
		}
	}

	public bool IsActive(){
		return menuActive;
	}
}
