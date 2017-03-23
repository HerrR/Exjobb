using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeTracker : MonoBehaviour {
	public GameObject currentTarget;
	
	// Update is called once per frame
	void Update () {
		if (Settings.selectionMode == "Gaze") {
			currentTarget = UpdateTarget ();
			DrawRay ();
			UpdateHovered ();
		}
	}

	public bool HasTarget(){
		return currentTarget != default(GameObject);
	}

	void UpdateHovered(){
		foreach (LayerImage layerImage in GameObject.FindObjectsOfType<LayerImage>()) {
			if(GameObject.Equals(layerImage.gameObject, currentTarget)){
				
				if(!layerImage.isHovered)
					layerImage.ToggleHovered();
				
			} else {
				
				if(layerImage.isHovered)
					layerImage.ToggleHovered();
				
			}
		}
	}

	GameObject UpdateTarget(){
		Vector3 fwd = transform.TransformDirection(Vector3.forward);

		RaycastHit[] hits;
		hits = Physics.RaycastAll (transform.position, fwd, 500);
		List<GameObject> imagesHit = new List<GameObject> ();

		foreach (RaycastHit hit in hits) {
			if (hit.collider.gameObject.tag == "Image") {
				imagesHit.Add (hit.collider.gameObject);
			}
		}

		ShadowImage[] shadowImages = GameObject.FindObjectsOfType<ShadowImage> ();

		for (int i = 0; i < shadowImages.Length; i++) {
			foreach (GameObject img in imagesHit) {
				if (shadowImages [i].trackedImage == img.GetComponent<Image> ()) {
					return img;
				}
			}
		}

		return default(GameObject);
	}

	void DrawRay(){
		Color rayColor;
		if (currentTarget == default(GameObject)) {
			rayColor = Color.yellow;
		} else {
			rayColor = Color.green;
		}
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), rayColor);
	}
}
