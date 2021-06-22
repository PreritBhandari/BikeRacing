using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/Spawnables/Nitro Boost")]
public class P_NitroBoost : MonoBehaviour 
{
    //how much speed add
    public int SpeedAddition = 25;

    //how much score added after collect this
    public int ScoreAddition = 125;

    public int BoosterMultiplier = 2;

    private Player player = null;
    private int _tempSpeed = 0;
    private int _tempReachSpeed = 0;
    private int _tmpCurrentBoosted = 1;

	// Use this for initialization
	void Start () 
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
            //store current player speed
            _tempSpeed = player.motoController.playerCurrentSpeed;

            //when nitro and max speed exeed the maximum total of these values the maximum value applied
            if (player.motoSettings.MaxSpeed + SpeedAddition <= _tempSpeed + SpeedAddition)
            {
                _tempReachSpeed = player.motoSettings.MaxSpeed + SpeedAddition;
                _tempReachSpeed -= _tempSpeed;
                StartCoroutine(addNitro());
            }
                
            //When not reached to maximum speed with collecting the nitro just nitro applied.
            if (player.motoSettings.MaxSpeed + SpeedAddition > _tempSpeed + SpeedAddition)
            {
                _tempReachSpeed = SpeedAddition;
                StartCoroutine(addNitro());
            }   
            
            player.motoControlsGUI.score += ScoreAddition;
        }
    }

    IEnumerator addNitro()
    {
        _tmpCurrentBoosted += BoosterMultiplier;
        player.motoController.playerCurrentSpeed += BoosterMultiplier;

        yield return null;

        if (_tmpCurrentBoosted <= _tempReachSpeed)
            StartCoroutine(addNitro());

        yield return null;
    }
}
