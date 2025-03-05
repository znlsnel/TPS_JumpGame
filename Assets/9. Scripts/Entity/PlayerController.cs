using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpPower;
	[SerializeField] float rotSpeed;

	private InputManager input;
	private Rigidbody _rigidbody;
	private Vector2 moveDir;
	private Vector3 targetRot;

	private void Awake()
	{
		input = InputManager.Instance;
		_rigidbody = GetComponent<Rigidbody>();
		input.move.action.performed +=  OnMoveInput;
		input.move.action.canceled +=  OnMoveInput;
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
	}

	void OnMoveInput(InputAction.CallbackContext context)
	{
		moveDir = context.ReadValue<Vector2>();
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
		SetVelocity(rotatedInputDir * moveSpeed);  
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
}
