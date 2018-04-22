using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

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
	public GameObject HitEffect;
	public AnimationClip HitAnimationClip;
	private AudioSource _thisAudio; //TODO:: refactor this later
	public AudioClip HitSound;
	
	
	Animator _animator;
	private Collider2D _ballCollider;
	private Collider2D _collider;
	private bool _isTappingAvailable = true;
	
	void Start()
	{
		_playerCurrentEnegy = _playerMaxEnegy;
		_ball = null;
		_ballCollider = null;
		_thisAudio = GetComponent<AudioSource>();
		_collider = GetComponent<BoxCollider2D>();
		if (HitEffect == null)
		{
			HitEffect = Instantiate(Resources.Load("Prefabs/HitEffect") as GameObject);
			HitEffect.SetActive(false);
		}
	}

	private void FixedUpdate()
	{
		var input = Input.GetAxisRaw("Horizontal");
		if (IsInTriggerRange)
		{
			DifferenceInPosition = _ballCollider.bounds.center - new Vector3(_collider.bounds.center.x, _collider.bounds.center.y, 0);
		}
		
		float targetVelocityX = input * moveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing, accelerationTimeGrounded);
		transform.position += Velocity;
		
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
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
			if (DifferenceInPosition.y < 2f && DifferenceInPosition.y > 1f)
			{
				GoodHit();
				transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Hit");
				StartCoroutine(EnableHitIndicatorAfterAnimation());
				_thisAudio.PlayOneShot(HitSound);
				return;
			}
			if (DifferenceInPosition.y < 1f && DifferenceInPosition.y > 0.5f)
			{
				
				GreatHit();
				transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Hit");
				StartCoroutine(EnableHitIndicatorAfterAnimation());
				_thisAudio.PlayOneShot(HitSound);
				return;
			}
			if (DifferenceInPosition.y <= 0.5f && DifferenceInPosition.y > -0.5f)
			{
				PerfectHit();
				transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Hit");
				StartCoroutine(EnableHitIndicatorAfterAnimation());
				_thisAudio.PlayOneShot(HitSound);
			}
			else
			{
				Missed();
			}
			
			transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Hit");
			
		}
	}

	private IEnumerator EnableHitIndicatorAfterAnimation()
	{
		yield return new WaitForSecondsRealtime(HitAnimationClip.length-0.01f);
		transform.GetChild(2).gameObject.SetActive(true);
		StartCoroutine(DisableHitIndicatorAfterCooldown());
	}

	private void Missed()
	{
		Score.Instance.AddHitCount(Score.Hit.Missed);
	}

	private void GoodHit()
	{
		_ball.GetComponent<Ball>()
			.AddForce(DifferenceInPosition.x + 0.05f * Random.Range(-5, 5) * 0.7f, 7.5f / DifferenceInPosition.y);
		Score.Instance.AddHitCount(Score.Hit.Good);
	}

	private void GreatHit()
	{
		_ball.GetComponent<Ball>()
			.AddForce(DifferenceInPosition.x + Random.Range(-3f, 3f) * 0.5f, 7.5f / DifferenceInPosition.y);
		Score.Instance.AddHitCount(Score.Hit.Great);
		AnimateGreatHit();
	}

	private void PerfectHit()
	{
		
		_ball.GetComponent<Ball>().AddForce(DifferenceInPosition.x + Random.Range(-1.2f, 1.2f) * 2f,
			7.5f / Mathf.Clamp(DifferenceInPosition.y,0.1f,100f));
		AnimatePerfectHit();
		Score.Instance.AddHitCount(Score.Hit.Perfect);
	}

	private void AnimatePerfectHit()
	{
		HitEffect.transform.position = transform.position + new Vector3(0, 0.45f,0 );
		HitEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		HitEffect.SetActive(true);
		HitEffect.GetComponent<Animator>().SetTrigger("Perfect");
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}
	
	private void AnimateGreatHit()
	{
		HitEffect.transform.position = transform.position;
		HitEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		HitEffect.SetActive(true);
		HitEffect.GetComponent<Animator>().SetTrigger("Great");
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
		yield return new WaitForSecondsRealtime(0.05f);
		transform.GetChild(2).gameObject.SetActive(false);
	}

	private IEnumerator ResetHitAnimation()
	{
		yield return  new WaitForSecondsRealtime(0.2f);
		transform.GetChild(0).GetComponent<Animator>().ResetTrigger("Hit");
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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3( DifferenceInPosition.x-1, DifferenceInPosition.y+transform.localPosition.y, 0), new Vector3(DifferenceInPosition.x+1, DifferenceInPosition.y+transform.localPosition.y,0));
	}

	private void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
