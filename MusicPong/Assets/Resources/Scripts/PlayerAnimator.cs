using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator _animator;

	void Start () {
		_animator = gameObject.GetComponent<Animator>();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ResetAnimationTrigger();
			_animator.SetTrigger("Hit");
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			ResetAnimationTrigger();
			_animator.SetTrigger("Release");
		}
	}

	private void ResetAnimationTrigger()
	{
		_animator.ResetTrigger("Hit");
		_animator.ResetTrigger("Release");
	}
}
