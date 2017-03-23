using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
	private Quaternion defaultAngle;
	private float angleSpan = 45f;
	private LeverMove leverMove;

	public Material defaultMaterial;
	public Material activeMaterial;

	private bool isActive = false;
	private Renderer rend;

	// Use this for initialization
	void Start () {
		defaultAngle = gameObject.transform.rotation;
		leverMove = GameObject.FindObjectOfType<LeverMove> ();
		rend = GetComponent<Renderer> ();
		// leverMove = gameObject.transform.parent.GetComponentInChildren<LeverMove> ();
		ResetLeverToBackPosition ();
		if (Settings.navigationMode != "Lever") {
			Debug.Log (Settings.navigationMode);
			DestroyLever ();	
		}
	}

	void Update(){
		AdaptMaterial ();
	}

	public void DestroyLever(){
		Destroy (gameObject.transform.parent.gameObject);
	}

	public void ResetLeverToBackPosition(){
		Quaternion backmostPosition = Quaternion.Euler (new Vector3 (-1 * angleSpan, -180, 0));
		transform.rotation = backmostPosition;
		leverMove.Move (GetLeverValue ());
		// transform.rotation = new Vector3 (-1*angleSpan, -180, 0); 
	}

	public void MoveLever(Vector3 _towardsPosition){
		Vector3 lookPos = _towardsPosition - transform.position;
		lookPos.x = 0;
		Quaternion rotation = Quaternion.LookRotation (lookPos);
		transform.rotation = rotation;
		leverMove.Move (GetLeverValue ());
		// Debug.Log(GetLeverValue ());
	}

	public float GetLeverValue(){
		float leverValue;
		if (transform.localRotation.eulerAngles.y == 0) {
			// Lever pulled forward
			leverValue = transform.rotation.eulerAngles.x - defaultAngle.eulerAngles.x;
		} else if (transform.localRotation.eulerAngles.y == 180) {
			// Lever pulled backwards
			leverValue = -1 * (transform.rotation.eulerAngles.x - defaultAngle.eulerAngles.x);
		} else {
			leverValue = 0f;
		}
		leverValue = Mathf.Clamp (leverValue, -1 * angleSpan, angleSpan);
		leverValue = (leverValue + angleSpan) / (2 * angleSpan);

		return leverValue;
	}

	public bool IsActive(){
		return isActive;
	}

	public void ToggleActive(){
		isActive = !isActive;
	}

	void AdaptMaterial(){
		if (isActive && (rend.material != activeMaterial)) {
			rend.material = activeMaterial;
		}

		if (!isActive && (rend.material != defaultMaterial)) {
			rend.material = defaultMaterial;
		}
	}
}
