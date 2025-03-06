using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : InteractivePlatform
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
				Vector3 v = rb.velocity;
				v.y = 0;
				rb.velocity = v;
				rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
			}
		}
	} 
}
