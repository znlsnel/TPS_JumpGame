using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
	[Range(1, 100)][SerializeField] private int health = 10;
	public int Health { get => health; set => health = Mathf.Clamp(value, 0, 100); }

	[Range(1f, 20f)][SerializeField] private float moveSpeed = 3; 
	public float MoveSpeed { get => moveSpeed; set => moveSpeed = Mathf.Clamp(value, 0, 100); }

	[Range(1f, 20f)][SerializeField] private float jumpPower = 10; 
	public float JumpPower { get => jumpPower; set => jumpPower = Mathf.Clamp(value, 0, 100); }

} 
