
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("info")]
	[SerializeField] private float rotSpeed = 540;
	 
	public LayerMask groundLayerMask;
	 
	private StatHandler statHandler;
	private PlayerDataHandler playerDataHandler;
	private AnimationHandler animHandler;
	private ClimbHandler climbHandler;
	private InputManager input;
	private Rigidbody _rigidbody;
	private Vector2 moveDir;
	private Vector3 targetRot;

	private bool wasGrounded = true;

	public StatHandler StatHandler => statHandler;
	public PlayerDataHandler PlayerDataHandler => playerDataHandler;
	private void Awake()
	{
		climbHandler = GetComponent<ClimbHandler>();
		playerDataHandler = GetComponent<PlayerDataHandler>();
		_rigidbody = GetComponent<Rigidbody>();
		statHandler = GetComponent<StatHandler>();
		animHandler = GetComponent<AnimationHandler>();


		wasGrounded = IsGrounded();
		InvokeRepeating(nameof(LandCheck), 0.1f, 0.1f);
	}
	private void Start()
	{
		InitInput();

	}
	void InitInput()
	{
		input = InputManager.Instance;

		input.Move.action.performed += OnMoveInput;
		input.Move.action.canceled += OnMoveInput;
		input.Jump.action.started += OnJumpInput;
	}

	private void OnDestroy() 
	{
		input.Move.action.performed -=  OnMoveInput;
		input.Move.action.canceled -=  OnMoveInput;
	}
	private void Update()
	{
		Rotate(targetRot);
	}

	private void FixedUpdate()
	{
		Move(moveDir);
		animHandler.Move(moveDir);
	}

	void OnMoveInput(InputAction.CallbackContext context)
	{
		moveDir = context.ReadValue<Vector2>();
		climbHandler.Move(moveDir.magnitude > 0f);
		if (moveDir.magnitude <= 0f && IsGrounded())
		{
			SetVelocity(_rigidbody.velocity / 3);   
			return;   
		}
	}
	void  OnJumpInput(InputAction.CallbackContext context)
	{
		if (IsGrounded())
		{
			_rigidbody.AddForce(Vector3.up * statHandler.JumpPower, ForceMode.Impulse);
			animHandler.Jump(); 
		}
	} 


	void Move(Vector2 dir)
	{  
		if (dir.magnitude <= 0f)
		{

			return; 
		}
		 
		 
		 
		Vector3 inputDir = new Vector3(dir.x, 0, dir.y);
		float cameraYaw = Camera.main.transform.eulerAngles.y;

		Quaternion yawRotation = Quaternion.Euler(0, cameraYaw, 0);
		Vector3 rotatedInputDir = yawRotation * inputDir; 

		Quaternion inputRotation = Quaternion.LookRotation(rotatedInputDir);
		targetRot = inputRotation.eulerAngles; // 최종 회전 각도

		SetVelocity(rotatedInputDir * statHandler.MoveSpeed); 
	}
	void Rotate(Vector3 rot)
	{
		if (moveDir.magnitude <= 0f) 
			return;

		Quaternion targetRotation = Quaternion.Euler(rot);
		float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
		float t = Mathf.Clamp01((rotSpeed * Time.deltaTime) / angleDifference);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
	}
	void SetVelocity(Vector3 v)
	{ 
	//	_rigidbody.MovePosition(transform.position + (v * 0.01f)) ;
		// _rigidbody.AddForce(v);
		_rigidbody.velocity = new Vector3(v.x, _rigidbody.velocity.y, v.z);
	}
	bool IsGrounded()
	{
		Ray[] rays = new Ray[4]
		{
			new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
			new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
			new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
			new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
		};

		for (int i = 0; i < rays.Length; i++)
		{
			if (Physics.Raycast(rays[i], 1f, groundLayerMask))
				return true;
		}
		return false;
	}
	void LandCheck()
	{

		bool isGround = IsGrounded();
		
		 
		// 걷다가 점프, 혹으 떨어지는 상황
		if (wasGrounded && !isGround)
		{
			animHandler.Falling();
			climbHandler.IsJump(true);
		}

		// 점프 후, 착지 상황
		else if (!wasGrounded && isGround) 
		{
			animHandler.Landing();
			climbHandler.IsJump(false);
		}

		wasGrounded = isGround;
	}
}
