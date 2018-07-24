using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	float accelerationTimeGrounded = 0.1f;
	private float moveSpeed = 0.1f;
	private float _buttoncooldown = 0.5f;
	private int _buttoncount = 0;
	private float _playerHp;
	private float _boosterCost;
	private float _rechargeRate;

	public Vector3 Velocity;
	private float _velocityXSmoothing;
	public GameObject Explosion;
	public bool IsInTriggerRange;
	public Vector2 DifferenceInPosition = new Vector2(100, 100);

	private Ball _ball;
	public GameObject HitEffect;
	public AnimationClip HitAnimationClip;
	private AudioSource _thisAudio; //TODO:: refactor this later
	public AudioClip HitSound;
	private Collider2D _ballCollider;
	private Collider2D _collider;
	private float _speedModifier = 1;
	private bool _isHoldingKey;
	private HitEffectAnimator _hitEffectAnimator;

	void Start()
	{
		_thisAudio = GetComponent<AudioSource>();
		_collider = GetComponent<BoxCollider2D>();
		if (HitEffect == null)
		{
			HitEffect = Instantiate(Resources.Load("Prefabs/HitEffect") as GameObject);
			_hitEffectAnimator = HitEffect.GetComponent<HitEffectAnimator>();
			HitEffect.SetActive(false);
		}
	}

	private void Update()
	{
		var input = Input.GetAxisRaw("Horizontal");
		float targetVelocityX = input * moveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing, accelerationTimeGrounded)*_speedModifier;
		transform.position += Velocity;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CheckForCollisionAndApplyForce();
		}
	}
	
	private void FixedUpdate()
	{
		if (IsInTriggerRange)
		{
			DifferenceInPosition =
				_ballCollider.bounds.center - new Vector3(_collider.bounds.center.x, _collider.bounds.center.y, 0);
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

	private void CheckForCollisionAndApplyForce()
	{
		if (DifferenceInPosition.y < 2f && DifferenceInPosition.y > 1f)
		{
			GoodHit();
			StartCoroutine(EnableHitIndicatorAfterAnimation());
			_thisAudio.PlayOneShot(HitSound);;
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
		if(_ball==null) return;
		_ball.AddForce(DifferenceInPosition.x + Random.Range(-1.2f, 1.2f) * 2f,
			7.5f / Mathf.Clamp(DifferenceInPosition.y,0.1f,100f), Score.Hit.Perfect);
		StartCoroutine(AnimatePerfectHit());
		gameObject.GetComponent<ScreenShake>().TriggerScreenShake();
		Score.Instance.AddHitCount(Score.Hit.Perfect);
	}

	private IEnumerator AnimatePerfectHit()
	{
		yield return new WaitForSecondsRealtime(HitAnimationClip.length-0.1f);
		_hitEffectAnimator.AnimatePerfectHitEffect(transform.position);
	}
	
	private IEnumerator AnimateGreatHit()
	{
		yield return new WaitForSecondsRealtime(HitAnimationClip.length-0.1f);
		_hitEffectAnimator.AnimateGreatHitEffect(transform.position);
	}

	public void  AnimateHitEffectAtBallExplode(Vector3 ballPosition)
	{
		_hitEffectAnimator.AnimateHitEffectAtBallExplode(ballPosition);
	}



	private IEnumerator DisableHitIndicatorAfterCooldown()
	{
		yield return new WaitForSecondsRealtime(0.025f);
		transform.GetChild(2).gameObject.SetActive(false);
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
}
