using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class RoadElement
{
    //just a name and just for identify the element here
    public String elementName = "roadElement";

    //the transform should contain meshlenght finder component on the root
    public Transform roadMesh = null;

    [HideInInspector]
    //the mesh lenght from mesh lenght finder component
    public float roadLenght = 0.0f;

    //Chance of spawn
    [Range (0,1)]
    public float spawnChance = 0.5f;
}

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/Road Generator")]
public class RoadGenerator : MonoBehaviour 
{
    //how many platform generate each time
    public int tileGenerateCount = 10;

    //distance for generate the tiles
    public float tileGenerateDistance = 12.0f;

    //The Road elements assignment
    public RoadElement[] roadElements = null;

    //Roads Lanes
    public Transform[] lanes = null;

    //The Road Slots for avoid double instantiate in lanes
    [HideInInspector]
    public Transform[] laneSlots = null;

    //Player setup
    private Player player;

    //current tile Index Storage
    private int tileIndex = 0;

    //temporary last road object
    private Transform tileTempTransform = null;

    //temporary value of the last block;
    private float tileTempPosition = 0;
    
    //Temporary float stores the distance value of previous tile
    private float prevTileDistance = 0.0f;

    private int prevIndex = 0;

    //tile counter
    private int counter = 0;

    //weight of chance
    private float _TotalWeight = 0.0f;

    // Use this for initialization
    void Start()
    {
        //get Player componet in scene and set as player
        player = (Player)FindObjectOfType(typeof(Player));

        //add lane slots in size of lanes for spawining item and avoid duplication in lane spawn
        Array.Resize(ref laneSlots, lanes.Length);

        //generate new roads (count) it set to 10 road platform now
        RoadCreator(tileGenerateCount);
    }

    void Awake()
    {
        _TotalWeight = 0.0f;
        foreach (var spawnable in roadElements)
        {
            _TotalWeight += spawnable.spawnChance;
        }

        //check all road elements for the lenght
        foreach (RoadElement r in roadElements)
        {
            if (r.roadMesh.GetComponent<MeshLenghtFinder>() != null)
            {
                //get the mesh lenght from mesh lenght finder
                r.roadLenght = r.roadMesh.GetComponent<MeshLenghtFinder>().getMeshLenght();
            } else {
                //when no mesh lenght finder is available on the mesh root, the transform removed from action
                r.roadMesh = null;
            }
            
        }
    }

    public int SpawnIndex()
    {
        float itemRandomWeight = UnityEngine.Random.value * _TotalWeight;
        int selectedIndex = 0;
        float weight = roadElements[0].spawnChance;

        while (itemRandomWeight > weight && selectedIndex < roadElements.Length - 1)
        {
            selectedIndex++;
            weight += roadElements[selectedIndex].spawnChance;
        }

        return selectedIndex;
    }

    void RoadCreator(int count)
    {
        //select Random tile from the road array
        tileIndex = SpawnIndex();

        if (prevIndex == tileIndex)
        {
            tileTempTransform = (Transform)
            Instantiate(roadElements[prevIndex].roadMesh,
            new Vector3(this.transform.position.x, this.transform.position.y, tileTempPosition + roadElements[tileIndex].roadLenght),
            this.transform.rotation);
        }
        else
        {
            tileTempTransform = (Transform)
            Instantiate(roadElements[prevIndex].roadMesh,
            new Vector3(this.transform.position.x, this.transform.position.y, tileTempPosition + roadElements[prevIndex].roadLenght),
            this.transform.rotation);
        }

        //get the current road Z and store it in temporary variable for future use
        tileTempPosition = tileTempTransform.position.z;

        prevIndex = tileIndex;

        //set the new platform name
        tileTempTransform.name = "RoadPlatform_" + counter.ToString();

        //it count +1 platform
        counter++;
    }

    void RoadReCreator()
    {
        //get the last platform game object
        GameObject lastPlatform = GameObject.Find("RoadPlatform_" + (counter - 1).ToString());

        //get temp gameobject for deletation
        GameObject tempPlatform = null;

        //check player is near the latest platform??
        if (player.transform.position.z > lastPlatform.transform.position.z - tileGenerateDistance)
        {
            //generate new roads (count) it set to 10 road platform now
            RoadCreator(tileGenerateCount);

            //get the last platforms generated and destroy them for manage memory and leave the 3 blocks alone
            for (int i = 0; i < counter - tileGenerateCount - 5; i++)
            {
                //find the first  platforms and then destroy the game object
                tempPlatform = GameObject.Find("RoadPlatform_" + i.ToString());

                //destroy the desired gameobject
                Destroy(tempPlatform);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //call distance checker of player each frame
        RoadReCreator();
    }
	
}
