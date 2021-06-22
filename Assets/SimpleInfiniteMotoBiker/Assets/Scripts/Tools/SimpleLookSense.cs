using UnityEngine;
using System.Collections;

[AddComponentMenu("Noir Project/Tools/Simple Camera Look Sense")]
public class SimpleLookSense : MonoBehaviour
{
	
	//define camera Sense
	public float CameraSensitivityX = 0.34f;
	public float CameraSensitivityY = 0.34f;
	
	//define min max of X
	public float CameraMinimumX = -15.0f;
	public float CameraMaximumX = 15.0f;
	
	//define min max of Y
	public float CameraMinimumY = -15.0f;
	public float CameraMaximumY = 15.0f;
	
	//define camera Rotation X Y
	private float CameraRotationY = 0.0f;
    private float CameraRotationX = 0.0f;

    //Mobile sense Multiplier
    public float MobileMultiplier = 5.0f;
	
	void CameraSense()
	{
		//Get Input X of mouse
		CameraRotationX += Input.GetAxis("Mouse X") * CameraSensitivityX;

        //get X input from mobile device
        CameraRotationX += Input.acceleration.x * CameraSensitivityX * MobileMultiplier;
        
		//Clamps a value between a minimum float and maximum float value. for Rotation of X
		CameraRotationX = Mathf.Clamp(CameraRotationX, CameraMinimumX, CameraMaximumX);	

		//Get Input Y of mouse
		CameraRotationY += Input.GetAxis("Mouse Y") * CameraSensitivityY;

        //get Z input of acceleration
        CameraRotationY += Input.acceleration.z * CameraSensitivityY * MobileMultiplier;

		//Clamps a value between a minimum float and maximum float value. for Rotation of Y
		CameraRotationY = Mathf.Clamp (CameraRotationY, CameraMinimumY, CameraMaximumY);
		
		//Make Camera Look to the desired TargetSense Look
		transform.localEulerAngles = new Vector3(-CameraRotationY, CameraRotationX, 0);
	}
	
	void Update ()
	{
		//call CameraSense Function
		CameraSense();
	}

}
