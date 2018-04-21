using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
	public GameObject Ball;

	void Start ()
	{
		if (Ball == null)
		{
			Ball = Instantiate(Resources.Load("Prefabs/Ball") as GameObject);
			Ball.transform.position = transform.position;
		}
		else
			Ball.transform.position = transform.position;
	}
}
