using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatformController : PlatformController
{
	[SerializeField] private float jumpPower;

	private static readonly int Push = Animator.StringToHash("Push");
	private Animator anim;

	protected override void Awake()
	{
		base.Awake();
		anim = GetComponent<Animator>();
	} 

	public override void EnterObject(GameObject go)
	{
		base.EnterObject(go);
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
