using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {

	float accelerationTimeAirborne = 0.2f; 		
	float accelerationTimeGrounded = 0.1f; 	
	private float moveSpeed = 0.1f;
	private float restartLevelDelay = 20f;
	private float _buttoncooldown = 0.5f;
	private int _buttoncount = 0;
	private float _playerHp;
	private float _playerMaxEnegy;
	private float _playerCurrentEnegy;
	private float _boosterCost;
	private float _rechargeRate;

	public Vector3 Velocity;
	private float _velocityXSmoothing;
	public GameObject Explosion;
	public bool IsInTriggerRange;
	public Vector2 BallPosition;
	public Vector2 DifferenceInPosition;
	private GameObject _ball;
	
	
	Animator _animator;
	private Collider2D _ballCollider;
	private Collider2D _collider;

	void Start()
	{
		_playerCurrentEnegy = _playerMaxEnegy;
		_ball = null;
		_ballCollider = null;
		_collider = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		var input = Input.GetAxisRaw("Horizontal");

		if (Input.GetKeyDown (KeyCode.Z)) {
			Restart();
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			Debug.LogWarning("fuck");
			CheckForCollisionAndApplyForce();
		}

		if (IsInTriggerRange)
		{
			Debug.LogError(DifferenceInPosition); 
			DifferenceInPosition =
				_ballCollider.bounds.center - new Vector3(_collider.bounds.center.x, _collider.bounds.center.y, 0);
		}
		float targetVelocityX = input * moveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing, accelerationTimeGrounded);
		transform.position += Velocity;
		
	}

	private void CheckForCollisionAndApplyForce()
	{
		if (IsInTriggerRange)
		{
			if (DifferenceInPosition.y < 1f && DifferenceInPosition.y > 0.8f)
			{
				Debug.LogWarning("no");
				_ball.GetComponent<Ball>().AddForce(DifferenceInPosition.x + 10f * Random.Range(-5, 5) * 10f,
					DifferenceInPosition.y * 1000);
				return;
			}
			if (DifferenceInPosition.y < 0.8f && DifferenceInPosition.y > 0.4f)
			{
				Debug.LogWarning("Good");
				_ball.GetComponent<Ball>()
					.AddForce(DifferenceInPosition.x + Random.Range(-3f, 3f) * 10f, DifferenceInPosition.y * 2000);
				return;
			}
			if (DifferenceInPosition.y <= 0.4f && DifferenceInPosition.y > 0)
			{
				Debug.LogWarning("perfect");
				_ball.GetComponent<Ball>().AddForce(DifferenceInPosition.x + Random.Range(-1.2f, 1.2f) * 10f,
					DifferenceInPosition.y * 3000);
			}
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		IsInTriggerRange = true;
		_ball = other.gameObject;
		_ballCollider = _ball.GetComponent<CircleCollider2D>();
	}

//	private void OnTriggerStay2D(Collider2D other)
//	{
//		DifferenceInPosition = _ballCollider.bounds.center - new Vector3(_collider.bounds.center.x,_collider.bounds.center.y,0);
//		Debug.LogError("diff"+DifferenceInPosition);
//	}

	private void OnTriggerExit2D(Collider2D other)
	{
		IsInTriggerRange = false;
		DifferenceInPosition = Vector2.zero;
		_ball = null;
	}

	private void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
