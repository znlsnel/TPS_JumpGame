using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("info")]
	[SerializeField] private float rotSpeed = 540;
	public LayerMask groundLayerMask;

	private AnimationHandler animHandler;
	private StatHandler statHandler;
	private InputManager input;
	private Rigidbody _rigidbody;
	private Vector2 moveDir;
	private Vector3 targetRot;

	private bool wasGrounded = true;



	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		statHandler = GetComponent<StatHandler>();
		animHandler = GetComponent<AnimationHandler>();

		input = InputManager.Instance;
		input.move.action.performed +=  OnMoveInput;
		input.move.action.canceled +=  OnMoveInput;
		input.jump.action.started += OnJumpInput;

		wasGrounded = IsGrounded();
		InvokeRepeating(nameof(LandCheck), 0.1f, 0.1f);
	} 

	private void OnDestroy() 
	{
		input.move.action.performed -=  OnMoveInput;
		input.move.action.canceled -=  OnMoveInput;
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
			return;
		

		Vector3 inputDir = new Vector3(dir.x, 0, dir.y);
		float cameraYaw = Camera.main.transform.eulerAngles.y;

		Quaternion yawRotation = Quaternion.Euler(0, cameraYaw, 0);

		// inputDir을 카메라의 yaw 각도만큼 회전시킴
		Vector3 rotatedInputDir = yawRotation * inputDir; 

		targetRot = new Vector3(0, cameraYaw, 0); 
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
		v.y = _rigidbody.velocity.y;
		_rigidbody.velocity = v;

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
		Ray ray = new Ray(transform.position, Vector3.down);
		bool isGround = Physics.Raycast(ray, 1f, groundLayerMask);

		// 걷다가 떨어지는 상황
		if (wasGrounded && !isGround)
		{
			animHandler.Falling(); 
		}

		// 점프 후, 착지 상황
		else if (!wasGrounded && isGround) 
		{
			animHandler.Landing();
		}

		wasGrounded = isGround;
	}
}
