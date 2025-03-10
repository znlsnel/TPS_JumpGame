using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	[Header("Bullet Info")]
	[SerializeField] private float damage;
	[SerializeField] private LayerMask targetLayer;

	[Header("Knock Back Info")]
	[SerializeField] private bool isOnKnockback = false;
	[SerializeField] private float knockbackTime = 0.5f;
	[SerializeField] private float knockbackPower = 0.1f;

	private Rigidbody _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if ((1 << other.gameObject.layer | targetLayer) == targetLayer)
		{ 
			if (isOnKnockback)
				other.GetComponent<PlayerController>().ApplyKnockback(_rigidbody.velocity, knockbackPower, knockbackTime);

			other.GetComponent<StatHandler>().OnDamage(damage);
			Debug.Log("¸Â¾Ò´Ù.");
		}  
	} 
}
