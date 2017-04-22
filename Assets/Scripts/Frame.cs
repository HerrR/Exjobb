using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frame : MonoBehaviour {
	public Sprite defaultFrame;
	public Sprite hoveredFrame;
	public Sprite activeFrame;

	private bool selected = false;
	private bool hovered = false;
	private Image frame;

	void Start () {
		frame = GetComponent<Image> ();
	}

	void Update () {
		AdaptImage ();
	}

	void AdaptImage(){
		if (selected) {
			frame.sprite = activeFrame;
		} else if (hovered) {
			frame.sprite = hoveredFrame;
		} else {
			frame.sprite = defaultFrame;
		}
	}

	public bool IsSelected(){
		return selected;
	}

	public bool IsHovered(){
		return hovered;
	}

	public void Select(){
		selected = true;
	}

	public void Deselect(){
		selected = false;
	}

	public void ToggleSelection(){
		selected = !selected;	
	}

	public void Hover(){
		hovered = true;
	}

	public void Unhover(){
		hovered = false;
	}

	public void ToggleHovered(){
		hovered = !hovered;
	}
}
