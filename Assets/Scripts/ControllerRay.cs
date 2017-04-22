using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRay : MonoBehaviour {
	private LineRenderer lineRenderer;

	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	public void UpdateRay(float _rayLength = 5f){
		lineRenderer.SetPosition (0, transform.position);
		Vector3 endPosition = new Vector3 ();
		endPosition = transform.position + transform.forward * _rayLength;
		lineRenderer.SetPosition (1, endPosition);
	}
}
