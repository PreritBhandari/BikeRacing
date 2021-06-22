using UnityEngine;
using System.Collections;

[AddComponentMenu("Noir Project/Tools/Disable Renderer")]
public class DisableRenderes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetComponent<Renderer>().enabled = false;
	}
}
