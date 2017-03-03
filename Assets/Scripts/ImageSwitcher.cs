using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour {

	public Sprite defaultImage;
	public Sprite hoverImage;
	public Sprite activeImage;

	private Image targetImage;
	private LayerImage layerImage;

	void Start () {
		targetImage = gameObject.GetComponent<Image> ();
		layerImage = gameObject.GetComponent<LayerImage> ();
	}

	void Update () {
		AdaptImage ();

		/*
		if (layerImage.isSelected && (targetImage.sprite == defaultImage)) {
			switchImage ();
		} else if (!layerImage.isSelected && (targetImage.sprite == activeImage)){
			switchImage ();
		}
		*/
	}

	void AdaptImage() {
		if (layerImage.isHovered && (targetImage.sprite != hoverImage)) {
			targetImage.sprite = hoverImage;
			return;
		}

		if (layerImage.isSelected && (targetImage.sprite != activeImage)) {
			targetImage.sprite = activeImage;
			return;
		}

		if(!layerImage.isSelected && !layerImage.isHovered && (targetImage.sprite != defaultImage)) {
			targetImage.sprite = defaultImage;
			return;
		}
	}
	/*
	public void switchImage() {
		if (layerImage.isSelected) {
			targetImage.sprite = activeImage;
		} else {
			targetImage.sprite = defaultImage;	
		}
	}
	*/
}
