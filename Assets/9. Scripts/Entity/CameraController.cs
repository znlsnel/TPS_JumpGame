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

		// �¿� ȸ�� (Y�� ȸ��)
		transform.Rotate(Vector3.up * mouseDelta.x);
		// ���� ȸ�� (X�� ȸ��) 
		rotationX -= mouseDelta.y; 
		rotationX = Mathf.Clamp(rotationX, minXRot, maxXRot); // ī�޶� �������� �ʵ��� ����

		// Quaternion�� ����Ͽ� ȸ�� ����
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
