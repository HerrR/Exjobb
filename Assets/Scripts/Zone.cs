using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

	[System.Serializable]
	public class zBounds {
		public float zMin;
		public float zMax;

		public zBounds(float _min, float _max){
			zMin = _min;
			zMax = _max;
		}
	}

	public string zoneName;
	public zBounds bounds;
	public string type;

	// Use this for initialization
	void Start () {
		BoxCollider boxCollider = gameObject.GetComponent<BoxCollider> ();

		if (boxCollider) {
			bounds = new zBounds (
				boxCollider.bounds.center.z - boxCollider.bounds.extents.z,
				boxCollider.bounds.center.z + boxCollider.bounds.extents.z

			);
		} else {
			Debug.LogError ("Box collider not found for " + name + " zone");
		}
	}
}
