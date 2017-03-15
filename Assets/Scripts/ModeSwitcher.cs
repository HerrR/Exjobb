using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class ModeSwitcher : MonoBehaviour {
	public string mode;

	// Assigned in inspector
	public GameObject textUpdateOnChange;

	// Auto assigned by tag
	public GameObject mainCanvas;

	// Auto assigned by tag 
	public GameObject generationZone;

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		generationZone = GameObject.FindGameObjectWithTag ("GenerationZone");
	}

	void OnTriggerEnter(Collider other) {
		if (!other.gameObject.GetComponent<Zone> ()) 
			return;

		if (other.gameObject.GetComponent<Zone> ().type != "mode")
			return;

		switchMode (other.gameObject.GetComponent<Zone> ().zoneName);

		if (textUpdateOnChange) {
			textUpdateOnChange.GetComponent<Text> ().text = other.gameObject.GetComponent<Zone>().zoneName;
		}
	}

	void switchMode(string _mode){
		mode = _mode;

		switch (mode){
		case "overview":
			mainCanvas.GetComponent<CompositeImage> ().ShowFrame ();
			List<Layer> allLayers = GameObject.FindObjectsOfType<Layer> ().ToList ();
			allLayers.Sort (Layer.SortLayerByBasePosZ);
			allLayers.Reverse ();
			foreach (Layer layer in allLayers) {
				layer.CopyToCanvas ();
				layer.DestroyShadowImage ();
				Destroy (layer.gameObject);
			}
			break;
		case "inspection":
			float zMin = generationZone.GetComponent<Zone> ().bounds.zMin;
			float zMax = generationZone.GetComponent<Zone> ().bounds.zMax;
			mainCanvas.GetComponent<CompositeImage> ().GenerateLayers (zMin, zMax);
			mainCanvas.GetComponent<CompositeImage> ().ClearCanvas ();
			mainCanvas.GetComponent<CompositeImage> ().HideFrame ();
			break;
		default:
			break;
		}
	}
}