using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/Spawnables/Traffic Car")]
public class P_TrafficCar : MonoBehaviour 
{
    public enum MovementDirection
    {
        Forward,
        Up,
        Right
    }
    public MovementDirection movementDirection = MovementDirection.Forward;

    public enum WheelRotationAxis
    {
        X,
        Y,
        Z
    }
    public WheelRotationAxis wheelRotationAxis = WheelRotationAxis.X;

    public Transform[] wheels = null;

    private float _initSpeedMin = 2.0f;
    private float _initSpeedMax = 4.0f;
    private float _initSpeed = 3.0f;

    private Player player = null;

    [HideInInspector]
    public bool isAlive = true;

	// Update is called once per frame
	void LateUpdate () 
    {
        if (player != null && isAlive && !player.motoController.isPaused)
        {
            //movement
            if (movementDirection == MovementDirection.Forward)
            {
                transform.Translate(Vector3.forward * _initSpeed * Time.deltaTime);

                //Destruct when too far in front
                if (transform.position.z >= player.transform.position.z + 45)
                {
                    Destroy(gameObject);
                }
            }
                
            if (movementDirection == MovementDirection.Right)
            {
                transform.Translate(Vector3.right * _initSpeed * Time.deltaTime);

                //Destruct when too far in front
                if (transform.position.x >= player.transform.position.x + 45)
                {
                    Destroy(gameObject);
                }
            }

            if (movementDirection == MovementDirection.Up)
            {
                transform.Translate(Vector3.up * _initSpeed * Time.deltaTime);

                //Destruct when too far in front
                if (transform.position.y >= player.transform.position.y + 45)
                {
                    Destroy(gameObject);
                }
            }

            //Rotate the Wheels
            WheelRotator();
        }
    }

    public void WheelRotator()
    {
        int RotationMultiplier = 50;
        float speedInTime = _initSpeed * RotationMultiplier * Time.deltaTime;

        if (wheels.Length > 0)
        {
            foreach (Transform t in wheels)
            {
                if (wheelRotationAxis == WheelRotationAxis.X)
                    t.transform.Rotate(Vector3.right * speedInTime);

                if (wheelRotationAxis == WheelRotationAxis.Y)
                    t.transform.Rotate(Vector3.up * speedInTime);

                if (wheelRotationAxis == WheelRotationAxis.Z)
                    t.transform.Rotate(Vector3.forward * speedInTime);
            }
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        //check if traffic car is inside this nitro booster
        if (collider.GetComponent<P_TrafficCar>() != null)
        {
            //kill the traffic car
            _initSpeed = 0;
        }

        //check if player is inside this nitro booster
        if (collider.GetComponent<Player>() != null)
        {
            //kill the player
            player.motoController.isAlive = false;
        }
    }

    void Start()
    {
        _initSpeed = Random.Range(_initSpeedMin, _initSpeedMax);
        player = (Player)FindObjectOfType(typeof(Player));
    }
}
