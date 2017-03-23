using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Layer : MonoBehaviour {
	public delegate void WhenMoveComplete();
	WhenMoveComplete whenMoveComplete;

	public IEnumerator movementRoutine;

	private LayerManager layerManager;
	private GameObject mainCanvas;
	private GameObject shadowCanvasBack;
	private GameObject shadowCanvasFront;

	private GameObject generationZone;

	public Vector3 basePosition;
	public bool accordion;
	private int accordionAmplifier = 200;

	private float zMax;
	private float zMin;

	public bool atMaxPosition = false;
	public bool atMinPosition = false;

	public Image layerImage;
	public Image shadowImageFront;
	public Image shadowImageBack;

	private Text layerText;

	private float movementSpeed = 1.5f;
	public bool moveFromToRunning;
	public bool accordionToggleInProcess;

	public Neighbours neighbours;

	[System.Serializable]
	public struct Neighbours{
		public Layer frontNeighbour, backNeighbour;

		public Neighbours(Layer _frontNeighbour, Layer _backNeighbour){
			this.frontNeighbour = _frontNeighbour;
			this.backNeighbour = _backNeighbour;
		}
	}

	public static int SortLayerByBasePosZ(Layer l1, Layer l2){
		return l1.basePosition.z.CompareTo (l2.basePosition.z);
	}

	void Awake() {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		shadowCanvasBack = GameObject.FindGameObjectWithTag ("ShadowCanvas");
		shadowCanvasFront = GameObject.FindGameObjectWithTag ("ShadowCanvasFront");

		generationZone = GameObject.FindGameObjectWithTag ("GenerationZone");
		layerManager = GameObject.FindObjectOfType<LayerManager> ();
		layerText = gameObject.GetComponentInChildren<Text> ();
	}

	void Start () {
		zMax = generationZone.GetComponent<Zone> ().bounds.zMax;
		zMin = generationZone.GetComponent<Zone> ().bounds.zMin;
		accordion = true;
		accordionToggleInProcess = false;
		moveFromToRunning = false;
		FindLayerImage ();
		GenerateShadowImages ();
	}

	public void GenerateShadowImages(){
		shadowImageBack = CreateShadowImage (shadowCanvasBack, layerImage);
		shadowImageFront = CreateShadowImage (shadowCanvasFront, layerImage);
	}

	void Update(){
		if (accordion) {
			AccordionMove ();
			ShowHideWhenZMaxOrMin ();
		}
	}

	public bool isSelected() {
		return layerImage.GetComponent<LayerImage> ().isSelected;
	}

	void FindLayerImage(){
		Image[] layerImages = gameObject.GetComponentsInChildren<Image> ();

		foreach (Image img in layerImages) {
			if (img.gameObject.tag == "Image") {
				layerImage = img;
			}
		}
	}

	public void ToggleAccordion(){
		if (accordionToggleInProcess) {
			return;
		}

		Vector3 startingPosition;
		Vector3 targetPosition;
		if (!accordion) {
			startingPosition = gameObject.transform.position;
			targetPosition = GetAccordionPosition ();
			whenMoveComplete += EnableAccordion;
		} else {
			startingPosition = GetAccordionPosition();
			targetPosition = basePosition;
			whenMoveComplete += DisableAccordion;
			accordion = false;
		}

		CallMoveFromTo(gameObject.transform,
			gameObject.transform.position,
			targetPosition,
			2.5f);
		
		accordionToggleInProcess = true;
	}

	public void EnableAccordion(){
		whenMoveComplete -= EnableAccordion;
		accordion = true;
		accordionToggleInProcess = false;
	}

	public void DisableAccordion(){
		whenMoveComplete -= DisableAccordion;
		accordion = false;
		accordionToggleInProcess = false;
	}

	void AccordionMove(){
		gameObject.transform.position = GetAccordionPosition();
	}

	public Vector3 GetAccordionPosition(){
		float offsetPos = accordionAmplifier * Mathf.Pow (
			(basePosition.z - Settings.layerMoveBaseCollider.gameObject.transform.position.z) - 0.3f
			, 7);
		Vector3 accordionPosition = gameObject.transform.position;
		accordionPosition.z = Mathf.Clamp ((basePosition.z + offsetPos), zMin, zMax);
		return accordionPosition;
	}

	public Image CreateShadowImage(GameObject _targetCanvas, Image _trackedImage){
		Vector3 pos = new Vector3 (
			layerImage.gameObject.transform.position.x,
			layerImage.gameObject.transform.position.y,
			_targetCanvas.transform.position.z);

		Image shadowImg = GameObject.Instantiate (
			layerImage, 
			pos,
			_targetCanvas.transform.rotation,
			_targetCanvas.transform);

		Destroy (shadowImg.GetComponent<LayerImage> ());
		Destroy (shadowImg.GetComponent<ImageSwitcher> ());
		shadowImg.gameObject.AddComponent<ShadowImage> ();
		shadowImg.GetComponent<ShadowImage> ().SetTrackedImage (_trackedImage);

		shadowImg.name = gameObject.name + " shadow image";
		shadowImg.tag = "ShadowImage";

		return shadowImg;
	}

	void ShowHideWhenZMaxOrMin(){
		if (gameObject.transform.position.z == zMax) {
			layerImage.enabled = false;
			HideLayerText ();
			atMaxPosition = true;
		} else if (gameObject.transform.position.z == zMin) {
			layerImage.enabled = false;
			HideLayerText ();
			atMinPosition = true;
		} else {
			layerImage.enabled = true;
			ShowLayerText ();
			atMaxPosition = false;
			atMinPosition = false;
		}
	}

	public void DestroyShadowImages(){
		Destroy (shadowImageBack.gameObject);
		Destroy (shadowImageFront.gameObject);
	}

	public bool HasShadowImages(){
		return ((shadowImageFront != default(Image)) || (shadowImageBack != default(Image)));
	}

	public void MoveZ(float _zCoord){
		Vector3 newPos = new Vector3 (
			                 gameObject.transform.position.x,
			                 gameObject.transform.position.y,
			                 _zCoord);
		gameObject.transform.position = newPos;
		neighbours = FindNeighbours ();
		CheckForLayerSwitch ();
		ShowHideWhenZMaxOrMin ();
	}

	public void CheckForLayerSwitch(){
		if ((neighbours.frontNeighbour != default(Layer)) && neighbours.frontNeighbour.basePosition.z < gameObject.transform.position.z) {
			SwitchPositionWithLayer (neighbours.frontNeighbour);
		}

		if ((neighbours.backNeighbour != default(Layer)) && neighbours.backNeighbour.basePosition.z > gameObject.transform.position.z) {
			SwitchPositionWithLayer (neighbours.backNeighbour);
		}
	}

	void SwitchPositionWithLayer(Layer target){
		float newBasePositionZ = target.basePosition.z;
		target.ChangeBasePositionZ (basePosition.z);
		ChangeBasePositionZ (newBasePositionZ);
		target.CallMoveFromTo(
			target.gameObject.transform, 
			target.gameObject.transform.position, 
			target.basePosition, 
			movementSpeed);
		layerManager.GenerateShadowImages ();
	}

	public void ChangeBasePositionZ(float _z){
		basePosition.z = _z;
	}

	public void UpdateLayerText(string _text){
		layerText.text = _text;
	}

	public void ShowLayerText(){
		layerText.enabled = true;
	}

	public void HideLayerText(){
		layerText.enabled = false;
	}

	public void CallMoveFromTo(Transform _objectToMove, Vector3 _a, Vector3 _b, float _speed){
		if (moveFromToRunning) {
			try {
				StopCoroutine (movementRoutine);
				moveFromToRunning = false;
			} catch {
				Debug.LogError ("Failed to stop coroutine", gameObject);	
			}
		}

		movementRoutine = MoveFromTo (_objectToMove, _a, _b, _speed);
		StartCoroutine (movementRoutine);
	}

	public IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed){
		moveFromToRunning = true;
		float step = (speed / (a - b).magnitude) * Time.deltaTime;
		float t = 0;
		while (t <= 1.0f) {
			t += step;
			objectToMove.position = Vector3.Lerp (a, b, t);
			ShowHideWhenZMaxOrMin ();
			yield return new WaitForFixedUpdate ();
		}
		objectToMove.position = b;
		moveFromToRunning = false;
		try {
			whenMoveComplete ();
		} catch {
			// Debug.Log ("No moveComplete delegate methods");
		}
	}

	Neighbours FindNeighbours(){
		List<Layer> allLayers = gameObject.transform.parent.GetComponentsInChildren<Layer> ().ToList ();
		allLayers.Sort (SortLayerByBasePosZ);

		for(int i = 0; i < allLayers.Count; i++){
			if (allLayers [i].gameObject.Equals (gameObject)) {

				Layer backNeighbour;
				if (i == 0) {
					// Last layer is selected, no back neighbour
					backNeighbour = null;
				} else {
					backNeighbour = allLayers [i - 1];
				}

				Layer frontNeighbour;
				if (i == (allLayers.Count - 1)) {
					// First layer is selected, no front neighbout
					frontNeighbour = null;
				} else {
					frontNeighbour = allLayers [i + 1];
				}

				return new Neighbours (frontNeighbour, backNeighbour);
			}
		}

		return new Neighbours ();
	}

	public Image CopyToCanvas(){
		layerImage.enabled = true;
		Quaternion canvasRotation = mainCanvas.transform.rotation;
		Vector3 spawnPosition = layerImage.transform.position;
		spawnPosition.z = mainCanvas.transform.position.z;

		Image imageCopy = Instantiate (
			layerImage,
			spawnPosition,
			canvasRotation,
			mainCanvas.transform
		);

		imageCopy.name = "Image";

		return imageCopy;
	}
}
