using UnityEngine;


public class ScreenShake : MonoBehaviour 
{
	
	Vector3 originalCameraPosition;
	
	public float shakeAmt;
	Camera mainCamera;

	void Start () {
		mainCamera = Camera.main;
		originalCameraPosition = mainCamera.transform.localPosition;
	}
	
	public void TriggerScreenShake()
	{
		InvokeRepeating ("CameraShake", 0, .15f);
		Invoke ("StopShaking", 0.2f);
	}

	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.localPosition;
			pp.x+= quakeAmt;
			pp.y+= quakeAmt/2;// can also add to x and/or z
			mainCamera.transform.localPosition = pp;
		}
	}
	
	void StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.localPosition = originalCameraPosition;
	}
	
}
