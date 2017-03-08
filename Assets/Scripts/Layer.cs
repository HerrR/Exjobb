using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Layer : MonoBehaviour {
	private GameObject mainCanvas;
	private GameObject mainCamera;

	private GameObject inspectionZone;
	private GameObject generationZone;

	public Vector3 basePosition;
	public bool accordion;

	private float zMax;
	private float zMin;
	public Image layerImage;
	private float movementSpeed = 1.5f;

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

	void Start () {
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas");
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		inspectionZone = GameObject.FindGameObjectWithTag ("InspectionZone");
		generationZone = GameObject.FindGameObjectWithTag ("GenerationZone");
		zMax = generationZone.GetComponent<Zone> ().bounds.zMax;
		zMin = generationZone.GetComponent<Zone> ().bounds.zMin;
		accordion = true;
		FindLayerImage ();
	}

	void Update(){
		if (accordion) {
			AccordionMove ();
		}
	}

	public bool isSelected() {
		return layerImage.GetComponent<LayerImage> ().isSelected;
	}

	void FindLayerImage(){
		Image[] layerImages = gameObject.GetComponentsInChildren<Image> ();

		foreach (Image img in layerImages) {
			if (img.gameObject.tag != "DontCopy") {
				layerImage = img;
			}
		}
	}

	void AccordionMove(){
		float offsetPos = 1000 * Mathf.Pow (
			(basePosition.z - mainCamera.transform.position.z) - 0.3f
			, 7);
		Vector3 _pos = gameObject.transform.position;
		_pos.z = Mathf.Clamp ((basePosition.z + offsetPos), zMin, zMax);

		gameObject.transform.position = _pos;
	}

	public void MoveZ(float _zCoord){
		Vector3 newPos = new Vector3 (
			                 gameObject.transform.position.x,
			                 gameObject.transform.position.y,
			                 _zCoord);
		gameObject.transform.position = newPos;
		neighbours = FindNeighbours ();
		CheckForLayerSwitch ();
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
		StartCoroutine(target.MoveFromTo(
			target.gameObject.transform, 
			target.gameObject.transform.position, 
			target.basePosition, 
			movementSpeed));
	}

	public void ChangeBasePositionZ(float _z){
		basePosition.z = _z;
	}

	public IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed){
		float step = (speed / (a - b).magnitude) * Time.deltaTime;
		float t = 0;
		while (t <= 1.0f) {
			t += step;
			objectToMove.position = Vector3.Lerp (a, b, t);
			yield return new WaitForFixedUpdate ();
		}
		objectToMove.position = b;
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

	public void copyToCanvas(){
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
	}
}
