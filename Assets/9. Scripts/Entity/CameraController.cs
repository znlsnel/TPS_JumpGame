using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraController : MonoBehaviour
{
	[Header ("Camear Option")]
	public float sensitivity = 100f;
	private float rotationX = 0f;
	public float minXRot = -50f;
	public float maxXRot = 50f; 
	Vector2 mouseDir;


	private void Awake()
	{
		InputManager.Instance.mouseMove.action.performed += MouseInput;
	}

	private void LateUpdate()
	{
		MoveCamera();
	}

	void MoveCamera()
	{
		Vector2 mouseDelta = mouseDir * sensitivity * Time.deltaTime;

		// 좌우 회전 (Y축 회전)
		transform.Rotate(Vector3.up * mouseDelta.x);
		// 상하 회전 (X축 회전) 
		rotationX -= mouseDelta.y; 
		rotationX = Mathf.Clamp(rotationX, minXRot, maxXRot); // 카메라가 뒤집히지 않도록 제한

		// Quaternion을 사용하여 회전 적용
		transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y, 0f);
		mouseDir = Vector2.zero; 
	}
	  
	void MouseInput(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Canceled)
		{
			mouseDir = Vector3.zero;
			return;
		}

		mouseDir = context.ReadValue<Vector2>();
		
	}
} 
