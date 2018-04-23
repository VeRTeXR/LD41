using UnityEngine;

public class BallSpawner : MonoBehaviour
{
	public GameObject Ball;
	public GameObject BallOriginal;
	public float BallExploded = 0;
	
	void Start ()
	{
		BallOriginal = Resources.Load("Prefabs/Ball") as GameObject;
		Ball = GameObject.FindGameObjectWithTag("Ball");
		if (Ball == null)
		{
			Ball = Instantiate(BallOriginal);
			Ball.transform.position = transform.position;
			Ball.transform.parent = Manager.Instance.GameplayArea.transform;
		}
		else
			Ball.transform.position = transform.position;
	}

	public void RetryRespawnBall()
	{
		BallExploded = 0;
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

	public void SpawnBall()
	{
		RandomSpawnerPosition();
		Ball = Instantiate(BallOriginal);
		Ball.transform.position = transform.position;
		Ball.transform.parent = Manager.Instance.GameplayArea.transform;
		BallExploded++;
	}

	private void RandomSpawnerPosition()
	{
		gameObject.transform.localPosition += new Vector3(Random.Range(-3f, 3f),0 ,0);
	}
}
