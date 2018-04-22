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
			Ball.transform.parent = Manager.Instance.GameplayArea.transform;
		}
		else
			Ball.transform.position = transform.position;
	}

	public void ResetBall()
	{
		Ball.transform.position = transform.position;
		Ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		Ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
	}
}
