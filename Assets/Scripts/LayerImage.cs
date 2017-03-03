using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerImage : MonoBehaviour {
	public bool isSelected;
	public bool isHovered;
	private float movementSpeed = 1500f;

	private RectTransform rectTransform;

	void Start() {
		rectTransform = gameObject.GetComponent<RectTransform>();
	}

	public void ToggleHovered() {
		isHovered = !isHovered;
	}

	public void ToggleSelection() {
		isSelected = !isSelected;
	}

	public void MoveImage(Vector2 direction){
		// Debug.Log ("Move image called with x: " + direction.x + " and y: " + direction.y, gameObject);

		float mainCanvasHeight = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<RectTransform> ().rect.height;
		float mainCanvasWidth = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<RectTransform> ().rect.width;

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
}
