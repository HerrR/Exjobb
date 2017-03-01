using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour {

	public Sprite defaultImage;
	public Sprite activeImage;
	public bool isSelected = false;
	private Image targetImage;

	void Start () {
		targetImage = gameObject.GetComponent<Image> ();
	}

	void Update () {
		if (isSelected && (targetImage.sprite == defaultImage)) {
			switchImage ();
		} else if (!isSelected && (targetImage.sprite == activeImage)){
			switchImage ();
		}
	}

	public void switchImage() {
		if (isSelected) {
			targetImage.sprite = activeImage;
		} else {
			targetImage.sprite = defaultImage;	
		}
	}
}
