using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class layer_overview : MonoBehaviour {
	public GameObject[] layers;
	
	// Use this for initialization
	void Start () {
		copyLayersToSelf ();
	}

	void copyLayersToSelf(){
		layers = GameObject.FindGameObjectsWithTag ("Layer").OrderByDescending (go => go.transform.position.z).ToArray ();
		foreach(GameObject layer in layers){
			if (layer.gameObject.transform.Find ("Image")) {
				Instantiate(layer.gameObject.transform.Find("Image"), transform.position, transform.rotation ,gameObject.transform);
			}
		}
	}
}
