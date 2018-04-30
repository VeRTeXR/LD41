using System.Collections;
using UnityEngine;

public class HitEffectAnimator : MonoBehaviour {
	private Animator _animator;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	public void AnimatePerfectHitEffect(Vector3 effectPosition)
	{
		transform.position = effectPosition + new Vector3(0, 0.45f,0 );
		transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		gameObject.SetActive(true);
		_animator.SetTrigger("Perfect");
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}
	
	public void AnimateGreatHitEffect(Vector3 effectPosition)
	{
		transform.position = effectPosition;
		transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		gameObject.SetActive(true);
		_animator.SetTrigger("Great");
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}
	
	public void  AnimateHitEffectAtBallExplode(Vector3 effectPosition)
	{
		transform.position = effectPosition + new Vector3(0, 0.45f,0 );
		transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		_animator.SetTrigger("Perfect");
		gameObject.SetActive(true);
		StartCoroutine(DisableHitEffectAfterAnimationPlayed());
	}
	
	private IEnumerator DisableHitEffectAfterAnimationPlayed()
	{
		yield return new WaitForSecondsRealtime(0.45f);
		_animator.ResetTrigger("Great");
		_animator.ResetTrigger("Perfect");
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
