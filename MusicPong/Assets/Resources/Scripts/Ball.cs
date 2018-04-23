using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private Rigidbody2D _rigidbody2D;
	private float _forceAddedCount;
	private float originalGravityScale;
	
	void Start ()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		originalGravityScale = _rigidbody2D.gravityScale;
	}

	public void AddForce(float forceX, float forceY, Score.Hit hitType = Score.Hit.Good)
	{
		if (hitType == Score.Hit.Great)
		{
			_forceAddedCount = _forceAddedCount + 2;
		}
		
		_rigidbody2D.velocity = Vector2.zero;
		_rigidbody2D.AddRelativeForce(new Vector2(forceX, Mathf.Clamp(forceY,0, 15f)),ForceMode2D.Impulse);
		_rigidbody2D.gravityScale = _rigidbody2D.gravityScale + Random.Range(-0.03f,0.03f);
	}

	void Update()
	{
		if (_forceAddedCount > 3)
		{
			StartCoroutine(DestroyAfterApplyForce());
			
		}
	}

	private IEnumerator DestroyAfterApplyForce()
	{
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
		FindObjectOfType<BallSpawner>().SpawnBall();
		FindObjectOfType<Player>().AnimateHitEffectAtBallExplode(transform.position);
		
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("Bound"))
		{
			_rigidbody2D.gravityScale = originalGravityScale;
			Manager.Instance.OutOfBound();
		}
	}
}
