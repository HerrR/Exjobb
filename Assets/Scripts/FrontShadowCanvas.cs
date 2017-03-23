using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontShadowCanvas : MonoBehaviour {
	public GameObject backCanvas;

	void Start () {
		FindBackCanvas ();
	}

	void FindBackCanvas(){
		backCanvas = GameObject.FindGameObjectWithTag ("ShadowCanvas");
	}

	void Update () {
		
	}
}
