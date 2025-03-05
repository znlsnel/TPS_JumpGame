using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraController : MonoBehaviour
{
	[Header("Info")]
	[SerializeField] Transform target;
	[SerializeField] private float minDistance;
	[SerializeField] private float maxDistance;
	[SerializeField] private float cameraDist = 3f;

	[Header ("Camear Option")]
	public float sensitivity = 100f;
	public float minXRot = -50f;
	public float maxXRot = 50f;

	private float rotationX = 0f;
	private Vector2 mouseDir;
	private Vector3 cameraDir;

	private void Awake()
	{ 
		cameraDir = (Camera.main.transform.localPosition - transform.localPosition).normalized; 
		InputManager.Instance.mouseMove.action.performed += MouseInput; 
		InputManager.Instance.mouseWheel.action.performed += MouseWheeInput;
	}

	private void LateUpdate()
	{
		MoveCamera();
		SetCameraDist();
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

	void MouseWheeInput(InputAction.CallbackContext context)
	{
		float y = context.ReadValue<Vector2>().y;
		cameraDist += Mathf.Clamp(-y, -1f, 1f);
		cameraDist = Mathf.Clamp(cameraDist, minDistance, maxDistance);
	}

	void MoveCamera()
	{
		transform.position = target.position;
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

	void SetCameraDist() 
	{
		float dist = cameraDist;
		Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
		Ray ray = new Ray(transform.position + Vector3.up * 0.2f, dir);  

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, cameraDist))
			dist = (hit.point - transform.position).magnitude;

		Camera.main.transform.localPosition = cameraDir * dist;
	} 
} 
