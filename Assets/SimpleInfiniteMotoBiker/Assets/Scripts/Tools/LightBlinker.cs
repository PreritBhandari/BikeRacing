using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Tools/Light Blinker")]
public class LightBlinker : MonoBehaviour {

    public float blinkDelaySeconds = 2.0f;
    private bool lightStatus = false;

    private float currentIntensity = 0;
    
    // Use this for initialization
	void Start () 
    {
        if (blinkDelaySeconds < 0)
            blinkDelaySeconds = 1.0f;

        if (GetComponent<Light>() != null)
        {
            currentIntensity = GetComponent<Light>().intensity;
            StartCoroutine(Blink());
        }
	}
	
	// Update is called once per frame
	IEnumerator Blink() 
    {
        yield return new WaitForSeconds(blinkDelaySeconds);

        if (lightStatus)
            GetComponent<Light>().intensity = currentIntensity;
        else
            GetComponent<Light>().intensity = 0;

        
        lightStatus = !lightStatus;
        yield return null;

        StartCoroutine(Blink());
        yield return null;
	}
}
