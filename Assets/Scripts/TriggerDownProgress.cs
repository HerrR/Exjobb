using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDownProgress : MonoBehaviour {
	private Image progressImage;
	private Text progressText;
	// Use this for initialization
	void Awake () {
		progressImage = GetComponentInChildren<Image> ();	
		progressText = GetComponentInChildren<Text> ();
	}

	public void Hide(){
		progressImage.enabled = false;
		progressText.enabled = false;
	}

	public void Show(){
		progressImage.enabled = true;
		progressText.enabled = true;
	}

	public void SetProgress(float _percentage){
		progressImage.fillAmount = _percentage;
	}
}
