using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowImage : MonoBehaviour {
	public Image trackedImage;
	public Image shadowImage;

	// Use this for initialization
	void Start () {
		shadowImage = gameObject.GetComponent<Image> ();
		shadowImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		CopyDataFromOriginal ();
		ShowHideShadow ();
	}

	public void SetTrackedImage(Image _targetImage){
		trackedImage = _targetImage;
	}

	void CopyDataFromOriginal() {
		shadowImage.sprite = trackedImage.sprite;
		shadowImage.rectTransform.anchoredPosition = trackedImage.rectTransform.anchoredPosition;
	}

	void ShowHideShadow(){
		shadowImage.enabled = !trackedImage.enabled;
	}
}
