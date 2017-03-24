using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeCrosshair : MonoBehaviour {

	void Start(){
		if (Settings.selectionMode != "Gaze") {
			HideCrosshair ();
		}
	}

	void Update(){
		MoveToFront ();	
	}

	public void MoveCrosshair(Vector3 _position){
		gameObject.transform.position = _position;
	}

	public void HideCrosshair(){
		GetComponent<Image> ().enabled = false;
	}

	public void ShowCrosshair(){
		GetComponent<Image> ().enabled = true;
	}

	public void MoveToFront(){
		transform.SetAsLastSibling ();
	}
}
