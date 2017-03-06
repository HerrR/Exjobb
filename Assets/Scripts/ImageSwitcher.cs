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
	}

	void AdaptImage() {
		if(layerImage.isSelected && layerImage.isHovered) {
			if (targetImage.sprite != activeImage) {
				targetImage.sprite = activeImage;
			}
			return;
		}

		if (layerImage.isSelected) {
			if (targetImage.sprite != activeImage) {
				targetImage.sprite = activeImage;
			}
			return;
		}

		if (layerImage.isHovered) {
			if (targetImage.sprite != hoverImage) {
				targetImage.sprite = hoverImage;
			}
			return;
		}

		if (targetImage.sprite != defaultImage) {
			targetImage.sprite = defaultImage;
			return;
		}
	}
}
