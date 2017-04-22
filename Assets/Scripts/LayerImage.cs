using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerImage : MonoBehaviour {
	public bool isSelected;
	public bool isHovered;
	// private float movementSpeed = 1500f;
	private GameObject mainCanvas;
	private RectTransform rectTransform;

	// Used for moving the image
	private Vector2 startingPosition;

	void Awake() {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
	}

	void Start() {
		rectTransform = gameObject.GetComponent<RectTransform>();
		UpdateStartingPosition ();
	}

	public void ToggleHovered() {
		isHovered = !isHovered;
	}

	public void ToggleSelection() {
		isSelected = !isSelected;
	}

	public void SetHovered(bool _hoverVal){
		isHovered = _hoverVal;
	}

	public void SetSelected(bool _selectedVal){
		isSelected = _selectedVal;
	}

	public void UpdateStartingPosition(){
		startingPosition = rectTransform.anchoredPosition;
	}

	public void MoveImageByVector(Vector2 _movementVector){
		Vector2 newPosition = startingPosition + _movementVector * 1000; // Need to multiply with 1000 because the canvas is downscaled to 0.001
		float mainCanvasHeight = mainCanvas.GetComponent<RectTransform> ().rect.height;
		float mainCanvasWidth = mainCanvas.GetComponent<RectTransform> ().rect.width;

		float minX = -1 * (mainCanvasWidth / 2 + rectTransform.rect.width / 2);
		float maxX =  mainCanvasWidth / 2 + rectTransform.rect.width / 2;

		float minY = -1 * (mainCanvasHeight / 2 + rectTransform.rect.height / 2);
		float maxY = mainCanvasHeight / 2 + rectTransform.rect.height / 2;

		newPosition.x = Mathf.Clamp (newPosition.x, minX, maxX);
		newPosition.y = Mathf.Clamp (newPosition.y, minY, maxY);

		rectTransform.anchoredPosition = newPosition;
	}
	/* 
	public void MoveImage(Vector2 direction){
		float mainCanvasHeight = mainCanvas.GetComponent<RectTransform> ().rect.height;
		float mainCanvasWidth = mainCanvas.GetComponent<RectTransform> ().rect.width;

		float minX = -1 * (mainCanvasWidth / 2 + rectTransform.rect.width / 2);
		float maxX =  mainCanvasWidth / 2 + rectTransform.rect.width / 2;

		float minY = -1 * (mainCanvasHeight / 2 + rectTransform.rect.height / 2);
		float maxY = mainCanvasHeight / 2 + rectTransform.rect.height / 2;

		Vector2 currentPosition = rectTransform.anchoredPosition;

		Vector2 newPosition = new Vector2 (
			Mathf.Clamp((currentPosition.x + direction.x * movementSpeed * Time.deltaTime), minX, maxX),
			Mathf.Clamp((currentPosition.y + direction.y * movementSpeed * Time.deltaTime), minY, maxY));

		rectTransform.anchoredPosition = newPosition;
	}
	*/
}
