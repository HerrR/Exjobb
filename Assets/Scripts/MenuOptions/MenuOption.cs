using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuOption : MonoBehaviour {
	public Sprite defaultImage, activeImage, hoverImage;
	public bool hovered, selected;

	private SpriteRenderer spriteRenderer;
	public Menu menu;
	private BoxCollider[] boxColliders;

	public bool showWhenMenuActive = true;

	public abstract void SelectOption ();

	public virtual void OnStart(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		menu = GetComponentInParent<Menu> ();
		boxColliders = GetComponents<BoxCollider> ();
	}

	public virtual void OnUpdate(){
		AdaptImage ();

		if (menu.IsActive () && showWhenMenuActive) {
			Show ();
		} else {
			Hide ();
		}
	}

	public virtual void AdaptImage(){
		if (IsSelected () && spriteRenderer.sprite != activeImage) {
			spriteRenderer.sprite = activeImage;
		} else if (IsHovered () && !IsSelected() && spriteRenderer.sprite != hoverImage) {
			spriteRenderer.sprite = hoverImage;
		} else if(!IsHovered() && !IsSelected() && spriteRenderer.sprite != defaultImage){
			spriteRenderer.sprite = defaultImage;
		}
	}

	public void SetHovered(bool _hovered){
		hovered = _hovered;
	}

	public bool IsSelected(){
		return selected;
	}

	public bool IsHovered(){
		return hovered;
	}

	public void Hide(){
		spriteRenderer.enabled = false;
		foreach (BoxCollider boxCollider in boxColliders) {
			boxCollider.enabled = false;
		}
	}

	public void Show(){
		spriteRenderer.enabled = true;
		foreach (BoxCollider boxCollider in boxColliders) {
			boxCollider.enabled = true;
		}
	}

	public bool IsShowing(){
		return spriteRenderer.enabled;
	}
}
