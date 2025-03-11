using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
	private static readonly int IsMoving = Animator.StringToHash("IsMove");
	private static readonly int IsJumping = Animator.StringToHash("IsJump");
	private static readonly int IsInAir = Animator.StringToHash("IsInAir");
	private static readonly int Climb = Animator.StringToHash("Climb");
	private static readonly int IsDie = Animator.StringToHash("IsDie");
	private static readonly int IsAlive = Animator.StringToHash("IsAlive");


	protected Animator animator;
	 
	private void Awake() 
	{
		animator = GetComponent<Animator>();
	}

	public void Move(Vector3 moveDir) => animator.SetBool(IsMoving, moveDir.magnitude > 0.5f); 
	public void Jump() => animator.SetTrigger(IsJumping);
	public void Landing() => animator.SetBool(IsInAir, false);  
	public void Falling() => animator.SetBool(IsInAir, true);
	public void OnClimb() => animator.SetTrigger(Climb);
	public void OnDie(bool active)
	{
		animator.SetBool(IsAlive, !active); 
		if (active)  
			animator.SetTrigger(IsDie);  
	}


}
