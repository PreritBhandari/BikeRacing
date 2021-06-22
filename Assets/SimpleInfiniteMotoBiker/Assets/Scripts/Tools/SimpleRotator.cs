using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Tools/Simple Rotator")]
public class SimpleRotator : MonoBehaviour {

    public enum RotationAxis
    {
        Right,
        Up,
        Forward
    }

    public RotationAxis rotationAxis = RotationAxis.Right;
    public float speed = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if (rotationAxis == RotationAxis.Right)
        {
            this.transform.Rotate(Vector3.right * speed * Time.deltaTime);
        }
        if (rotationAxis == RotationAxis.Up)
        {
            this.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
        if (rotationAxis == RotationAxis.Forward)
        {
            this.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
	}
}
