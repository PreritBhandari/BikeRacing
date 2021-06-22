using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/Spawner")]
public class SpawnManager : MonoBehaviour 
{
    [Serializable]
    public class SpawnItem
    {
        //this name is just for more organized system
        public string itemName = "ItemName";

        //item transform and mesh
        public Transform itemTransform = null;

        //When the spawner spawn the surface this value is for Y position, this will handle how far higher from the road
        //The X is grabbed from the SLOT points on road generator and the Z is set from each platform spawner individually
        public float spawnYAlign = 0.03f;

        //chance of spawn item
        [Range (0.0f, 1.0f)]
        public float SpawnChance = 1.0f;
    }

    private RoadGenerator roadGenerator = null;
    private Player player = null;

    private int laneIndex = 0;
    private float _TotalWeight = 0.0f;

    //flag for is spawning
    private bool spawntime = false;

    [Range (5.0f, 65.0f)]
    public float SpawnDistance = 25.0f;

    [Range (10.0f, 100.0f)]
    public float DestructDistance = 20.0f;

    //the speed addition to minimum speed for start spawning items 
    public int speedThreshold = 8;

    public DistanceDestruct.Axis DestructAxis = DistanceDestruct.Axis.Z;

    public float spawnTimeSecondMin = 3.0f;
    public float spawnTimeSecondMax = 15.0f;

    public SpawnItem[] spawnableItems;

    // Use this for initialization
    void Start()
    {
        //find road generator in scene
        roadGenerator = (RoadGenerator)FindObjectOfType(typeof(RoadGenerator));
        player = (Player)FindObjectOfType(typeof(Player));

        //if the roadgenerator or player isnt available in scene or this component can't reach that, 
        //destroy self
        if (roadGenerator == null || player == null)
            Destroy(this);
    }

	void Awake () {
        _TotalWeight = 0.0f;
        foreach (var spawnable in spawnableItems)
        {
            _TotalWeight += spawnable.SpawnChance;
        }    
	}

    //random number of seconds between min and max spawn second
    public float timeToSpawn()
    {
        return UnityEngine.Random.Range(spawnTimeSecondMin, spawnTimeSecondMax);
    }

    //Spawner
    IEnumerator Spawn(int laneSlot)
    {
        spawntime = true;
        yield return new WaitForSeconds(timeToSpawn());

        int spawnIndex = SpawnIndex();

        Transform newObject = (Transform)Instantiate(spawnableItems[spawnIndex].itemTransform, 
                                                      new Vector3 (roadGenerator.lanes[laneSlot].position.x,
                                                                   //The Y value is from lane slot position Y and spawnPointY in addition
                                                                   roadGenerator.lanes[laneSlot].position.y + spawnableItems[spawnIndex].spawnYAlign,
                                                                    //get the spawn z position from player z 
                                                                    //and then add SpawnDistance to this
                                                                   player.transform.position.z + SpawnDistance),
                                                      spawnableItems[spawnIndex].itemTransform.rotation
            );

        newObject.name = "Spawned-" + spawnableItems[spawnIndex].itemName;

        //remove this element using distance destruct tool after 20 unit pass on z of player
        newObject.gameObject.AddComponent<DistanceDestruct>();
        newObject.GetComponent<DistanceDestruct>().distance = DestructDistance;
        newObject.GetComponent<DistanceDestruct>().axis = DestructAxis;
        newObject.GetComponent<DistanceDestruct>().startDestruct = true;

        spawntime = false;
        yield return null;
    }

    public int SpawnIndex()
    {
        float itemRandomWeight = UnityEngine.Random.value * _TotalWeight;
        int selectedIndex = 0;
        float weight = spawnableItems[0].SpawnChance;

        while (itemRandomWeight > weight && selectedIndex < spawnableItems.Length - 1)
        {
            selectedIndex++;
            weight += spawnableItems[selectedIndex].SpawnChance;
        }

        return selectedIndex;
    }

    //check road generator for free lane and return free lane index
    public int findFreeLane()
    {
        //when no lane is detected return
        if (roadGenerator.lanes.Length <= 0)
        {
            Debug.Log("No Lane detected on the road Generator, Please define road lanes.");
            return -1;
        }

        int checkCount = 0;
        //when all of the lane slots are full return
        foreach (Transform t in roadGenerator.laneSlots)
        {
            //when a transform is null counter add 
            if (t != null)
            {
                checkCount++;
            }

            //when all lanes are full so return -1
            if (checkCount == roadGenerator.laneSlots.Length)
                return -1;
        }

        int lane = 0;
        lane = UnityEngine.Random.Range(0, roadGenerator.laneSlots.Length);

        if (roadGenerator.laneSlots[lane] == null)
            return lane;
        else
            findFreeLane();

        return -1;
    }

	// Update is called once per frame
	void Update () 
    {
        //when the time is come spawn new prefab
        int laneSlot = findFreeLane();

        //when the lanes are empty and nothing to spawn and player speed is +speedThreshold from minimum speed
        if (laneSlot != -1 && !spawntime && player.motoController.playerCurrentSpeed > player.motoSettings.MinSpeed + speedThreshold)
        {
            StartCoroutine(Spawn(laneSlot));
        }
	}
}
