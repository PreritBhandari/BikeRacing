using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Tools/Distance Destruct")]
public class DistanceDestruct : MonoBehaviour {

    public float distance = 15.0f;
    public bool startDestruct = false;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public Axis axis = Axis.Y;
    public bool inverseDirection = false;

    private Vector3 playerPos = Vector3.zero;
    private Player player = null;

	// Use this for initialization
	void Start () 
    {
        player = (Player) FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if (player != null && startDestruct)
        {
            playerPos = player.transform.position;

            if (inverseDirection)
                distance *= -1;

            if (axis == Axis.X)
            {
                if (playerPos.x >= this.transform.position.x + distance)
                {
                    Destroy(gameObject);
                }
            }

            if (axis == Axis.Y)
            {
                if (playerPos.y >= this.transform.position.y + distance)
                {
                    Destroy(gameObject);
                }
            }

            if (axis == Axis.Z)
            {
                if (playerPos.z >= this.transform.position.z + distance)
                {
                    Destroy(gameObject);
                }
            }
        }
	}
}
