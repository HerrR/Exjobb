using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour {

	public Sprite defaultImage;
	public Sprite activeImage;
	// public bool isSelected = false;
	private Image targetImage;
	private LayerImage layerImage;

	void Start () {
		targetImage = gameObject.GetComponent<Image> ();
		layerImage = gameObject.GetComponent<LayerImage> ();
	}

	void Update () {
		if (layerImage.isSelected && (targetImage.sprite == defaultImage)) {
			switchImage ();
		} else if (!layerImage.isSelected && (targetImage.sprite == activeImage)){
			switchImage ();
		}
	}

	public void switchImage() {
		if (layerImage.isSelected) {
			targetImage.sprite = activeImage;
		} else {
			targetImage.sprite = defaultImage;	
		}
	}
}
