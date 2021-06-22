using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/Spawnables/Wooden Road Block")]

public class P_WoodBlocker : MonoBehaviour {

    private Player player = null;

    // Use this for initialization
    void Start()
    {
        //get Player componet in scene and set as player
        player = (Player)FindObjectOfType(typeof(Player));
    }

    //When player enter the trigger
    void OnTriggerEnter(Collider collider)
    {
        //check if player is inside this nitro booster
        if (collider.GetComponent<Player>() != null)
        {
            //kill the player
            player.motoController.isAlive = false;
        }

        //check if traffic car is inside this nitro booster
        if (collider.GetComponent<P_TrafficCar>() != null)
        {
            //kill the traffic car
            collider.GetComponent<P_TrafficCar>().isAlive = false;
        }
    }
}
