using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : Platform
{
	[SerializeField] private float jumpPower;

	private static readonly int Push = Animator.StringToHash("Push");
	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
	 
	protected override void OnPlatformImpact()
	{
		anim.SetTrigger(Push); 
	}
	 
	public void AE_OnPush() 
	{
		foreach (var target in targets)
		{
			Rigidbody rb = target.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.velocity = Vector3.zero;
				rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
			} 
		}
	} 
}
