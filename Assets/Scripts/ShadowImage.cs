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
		RemoveBoxColliders ();
	}
	
/*	public void CreateBackImage(){
		GameObject frontShadowCanvas = GameObject.FindGameObjectWithTag ("ShadowCanvasFront");
		Vector3 pos = new Vector3 (
			shadowImage.gameObject.transform.position.x,
			shadowImage.gameObject.transform.position.y,
			frontShadowCanvas .transform.position.z
		);

		shadowImage = GameObject.Instantiate (
			shadowImage,
			pos,
			frontShadowCanvas.transform.rotation,
			frontShadowCanvas.transform
		);

		Destroy (shadowImage.GetComponent<LayerImage> ());
		Destroy (shadowImage.GetComponent<ImageSwitcher> ());

	}*/

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


	void RemoveBoxColliders(){
		BoxCollider[] boxColliders = shadowImage.GetComponents<BoxCollider> ();
		foreach (BoxCollider bc in boxColliders) {
			Destroy (bc);
		}
	}
}
