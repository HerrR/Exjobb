using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ImageToCanvas(Canvas targetCanvas){
		Debug.Log ("I am a layer and I want to copy my images to a canvas called " + targetCanvas.gameObject.name);
	}
}
