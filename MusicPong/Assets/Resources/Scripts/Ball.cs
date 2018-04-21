using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

	private Rigidbody2D _rigidbody2D;
	private Collider2D _collider2D; 
	
	
	// Use this for initialization
	void Start ()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_collider2D = GetComponent<Collider2D>();
	}

	public void AddForce(float forceX, float forceY)
	{
//		Debug.LogError(forceX+ " : "+forceY);
		_rigidbody2D.AddForce(new Vector2(forceX, forceY));
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("Bound"))
		{
//			Debug.LogError("11111");
			Manager.instance.OutOfBound();
		}
	}
}
