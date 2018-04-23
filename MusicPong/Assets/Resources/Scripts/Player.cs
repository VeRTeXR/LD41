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
	private float _boosterCost;
	private float _rechargeRate;

	public Vector3 Velocity;
	private float _velocityXSmoothing;
	public GameObject Explosion;
	public bool IsInTriggerRange;
	public Vector2 DifferenceInPosition;
	private Ball _ball;
	public GameObject HitEffect;
	public AnimationClip HitAnimationClip;
	private AudioSource _thisAudio; //TODO:: refactor this later
	public AudioClip HitSound;
	
	
	Animator _animator;
	private Collider2D _ballCollider;
	private Collider2D _collider;
	private bool _isTappingAvailable = true;
	private float _speedModifier = 1;

	void Start()
	{
		_thisAudio = GetComponent<AudioSource>();
		_collider = GetComponent<BoxCollider2D>();
		if (HitEffect == null)
		{
			HitEffect = Instantiate(Resources.Load("Prefabs/HitEffect") as GameObject);
			HitEffect.SetActive(false);
		}
	}

	private void Update()
	{
		var input = Input.GetAxisRaw("Horizontal");
		float targetVelocityX = input * moveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing, accelerationTimeGrounded)*_speedModifier;
		transform.position += Velocity;
		
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
			transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Hit");
		
	}
	
	private void FixedUpdate()
	{
		if (IsInTriggerRange)
		{
			DifferenceInPosition =
				_ballCollider.bounds.center - new Vector3(_collider.bounds.center.x, _collider.bounds.center.y, 0);
		}

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonUp(0))
		{
			CheckForCollisionAndApplyForce();
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			_speedModifier = 1.25f;
		}
		else
		{
			_speedModifier = 1;
		}
	}

	private IEnumerator ResetTapStatus()
	{
		yield return new WaitForSecondsRealtime(0.0025f);
		_isTappingAvailable = true;
	}

	private void CheckForCollisionAndApplyForce()
	{
		if (DifferenceInPosition.y < 2f && DifferenceInPosition.y > 1f)
		{
			GoodHit();
			StartCoroutine(EnableHitIndicatorAfterAnimation());
			_thisAudio.PlayOneShot(HitSound);
			return;
		}
		if (DifferenceInPosition.y < 1f && DifferenceInPosition.y > 0.5f)
		{
			GreatHit();
			StartCoroutine(EnableHitIndicatorAfterAnimation());
			_thisAudio.PlayOneShot(HitSound);
			return;
		}
		if (DifferenceInPosition.y <= 0.5f && DifferenceInPosition.y > -0.5f)
		{
			PerfectHit();
			StartCoroutine(EnableHitIndicatorAfterAnimation());
			_thisAudio.PlayOneShot(HitSound);
		}
		else
		{
			Missed();
		}
		
	}

	private IEnumerator EnableHitIndicatorAfterAnimation()
	{
		transform.GetChild(2).gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(HitAnimationClip.length-0.01f);
		StartCoroutine(DisableHitIndicatorAfterCooldown());
	}

	private void Missed()
	{
		Score.Instance.AddHitCount(Score.Hit.Missed);
	}

	private void GoodHit()
	{
		_ball.AddForce(DifferenceInPosition.x + 0.05f * Random.Range(-5, 5) * 0.7f, 7.5f / DifferenceInPosition.y,Score.Hit.Good);
		Score.Instance.AddHitCount(Score.Hit.Good);
	}

	private void GreatHit()
	{
		_ball.AddForce(DifferenceInPosition.x + Random.Range(-3f, 3f) * 0.5f, 7.5f / DifferenceInPosition.y,Score.Hit.Great);
		Score.Instance.AddHitCount(Score.Hit.Great);
		StartCoroutine(AnimateGreatHit());
	}

	private void PerfectHit()
	{
		_ball.AddForce(DifferenceInPosition.x + Random.Range(-1.2f, 1.2f) * 2f,
			7.5f / Mathf.Clamp(DifferenceInPosition.y,0.1f,100f), Score.Hit.Perfect);
		StartCoroutine(AnimatePerfectHit());
		gameObject.GetComponent<ScreenShake>().TriggerScreenShake();
		Score.Instance.AddHitCount(Score.Hit.Perfect);
	}

	private IEnumerator AnimatePerfectHit()
	{
		yield return new WaitForSecondsRealtime(HitAnimationClip.length-0.1f);
		HitEffect.transform.position = transform.position + new Vector3(0, 0.45f,0 );
		HitEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		HitEffect.SetActive(true);
		HitEffect.GetComponent<Animator>().SetTrigger("Perfect");
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}
	
	private IEnumerator AnimateGreatHit()
	{
		yield return new WaitForSecondsRealtime(HitAnimationClip.length-0.1f);
		HitEffect.transform.position = transform.position;
		HitEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		HitEffect.SetActive(true);
		HitEffect.GetComponent<Animator>().SetTrigger("Great");
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}

	public void  AnimateHitEffectAtBallExplode(Vector3 ballPosition)
	{
		HitEffect.transform.position = ballPosition;
		HitEffect.transform.position = ballPosition + new Vector3(0, 0.45f,0 );
		HitEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		HitEffect.GetComponent<Animator>().SetTrigger("Perfect");
		HitEffect.SetActive(true);
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}

	private IEnumerator DisableHitEffectAfterAnimationPlayed()
	{
		yield return new WaitForSecondsRealtime(0.45f);
		HitEffect.GetComponent<Animator>().ResetTrigger("Great");
		HitEffect.GetComponent<Animator>().ResetTrigger("Perfect");
		HitEffect.SetActive(false);
	}

	private IEnumerator DisableHitIndicatorAfterCooldown()
	{
		yield return new WaitForSecondsRealtime(0.025f);
		transform.GetChild(2).gameObject.SetActive(false);
	}

	private void ResetHitAnimation()
	{
		transform.GetChild(1).GetComponent<Animator>().ResetTrigger("Hit");
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_ball == null)
			_ball = other.GetComponent<Ball>();
		if(_ballCollider == null)
			_ballCollider = _ball.GetComponent<CircleCollider2D>();
		IsInTriggerRange = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		IsInTriggerRange = false;
		DifferenceInPosition = Vector2.zero;
		_ball = null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3( DifferenceInPosition.x-1, DifferenceInPosition.y+transform.localPosition.y, 0), new Vector3(DifferenceInPosition.x+1, DifferenceInPosition.y+transform.localPosition.y,0));
	}

	private void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
