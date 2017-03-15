using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
	private Quaternion defaultAngle;
	private float angleSpan = 45f;
	private LeverMove leverMove;

	// Use this for initialization
	void Start () {
		defaultAngle = gameObject.transform.rotation;
		leverMove = gameObject.transform.parent.GetComponentInChildren<LeverMove> ();
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


}
