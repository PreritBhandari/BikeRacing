using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Tools/Self Destruct")]
public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
		
	}
	
    public float timeSeconds = 0;

    IEnumerator destrcut()
    {
        yield return new WaitForSeconds(timeSeconds);
        Destroy(gameObject);
    }

    public void destructThis()
    {
        StartCoroutine(destrcut());
    }

	// Update is called once per frame
	void Update () {
		
	}
}
