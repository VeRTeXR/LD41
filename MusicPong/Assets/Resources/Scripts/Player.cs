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
	private bool _isTappingAvailable = true;
	
	void Start()
	{
		_playerCurrentEnegy = _playerMaxEnegy;
		_ball = null;
		_ballCollider = null;
		_collider = GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
		var input = Input.GetAxisRaw("Horizontal");

		if (Input.GetKeyDown (KeyCode.Z)) {
			Restart();
		}

		if (IsInTriggerRange)
		{
			DifferenceInPosition = _ballCollider.bounds.center - new Vector3(_collider.bounds.center.x, _collider.bounds.center.y, 0);
		}
		
		float targetVelocityX = input * moveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing, accelerationTimeGrounded);
		transform.position += Velocity;
		
		if (Input.GetKeyDown (KeyCode.Space))
		{
			if (_isTappingAvailable)
			{
				CheckForCollisionAndApplyForce();
				StartCoroutine(ResetTapStatus());
				_isTappingAvailable = false;
			}
		}
	}

	private IEnumerator ResetTapStatus()
	{
		yield return new WaitForSecondsRealtime(0.025f);
		_isTappingAvailable = true;
	}

	private void CheckForCollisionAndApplyForce()
	{
		if (IsInTriggerRange)
		{
			if (DifferenceInPosition.y < 1f && DifferenceInPosition.y > 0.8f)
			{
				Debug.LogWarning("no");
				GoodHit();
				return;
			}
			if (DifferenceInPosition.y < 0.8f && DifferenceInPosition.y > 0.4f)
			{
				Debug.LogWarning("Good");
				GreatHit();
				return;
			}
			if (DifferenceInPosition.y <= 0.4f && DifferenceInPosition.y > 0f)
			{
				Debug.LogWarning("perfect");
				PerfectHit();
			}
			else
			{
				Missed();
			}
		}
	}

	private void Missed()
	{
		Score.Instance.AddHitCount(Score.Hit.Missed);
	}

	private void GoodHit()
	{
		_ball.GetComponent<Ball>()
			.AddForce(DifferenceInPosition.x + 0.05f * Random.Range(-5, 5) * 0.7f, 5 / DifferenceInPosition.y);
		Score.Instance.AddHitCount(Score.Hit.Good);
	}

	private void GreatHit()
	{
		_ball.GetComponent<Ball>()
			.AddForce(DifferenceInPosition.x + Random.Range(-3f, 3f) * 0.5f, 5 / DifferenceInPosition.y);
		Score.Instance.AddHitCount(Score.Hit.Great);
	}

	private void PerfectHit()
	{
		_ball.GetComponent<Ball>().AddForce(DifferenceInPosition.x + Random.Range(-1.2f, 1.2f) * 2f,
			5 / DifferenceInPosition.y);
		Score.Instance.AddHitCount(Score.Hit.Perfect);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		IsInTriggerRange = true;
		_ball = other.gameObject;
		_ballCollider = _ball.GetComponent<CircleCollider2D>();
	}

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
