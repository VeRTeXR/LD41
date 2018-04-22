using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
	public GameObject Ball;
	public GameObject BallOriginal;
	void Start ()
	{
		BallOriginal = Resources.Load("Prefabs/Ball") as GameObject;
		Ball = GameObject.FindGameObjectWithTag("Ball");
		if (Ball == null)
		{
			Ball = Instantiate(BallOriginal);
			Ball.transform.position = transform.position;
			Debug.LogError(Manager.Instance.GameplayArea);
			Ball.transform.parent = Manager.Instance.GameplayArea.transform;
		}
		else
			Ball.transform.position = transform.position;
	}

	public void RetryRespawnBall()
	{
		if (Ball == null)
		{
			Ball = Instantiate(BallOriginal);
			Ball.transform.position = transform.position;
			Ball.transform.parent = Manager.Instance.GameplayArea.transform;
		}
	}

	public void ResetBall()
	{
		Ball.transform.position = transform.position;
		Ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		Ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
	}
}
