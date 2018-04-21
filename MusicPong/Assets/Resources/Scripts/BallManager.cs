using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{

	public static BallManager Instance = null;
	public GameObject BallSpawner;
	
	void Start () {
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		if (FindObjectOfType<BallSpawner>() == null)
		{
			
		}	
	}

	public void ResetBall()
	{
		
	}
}
