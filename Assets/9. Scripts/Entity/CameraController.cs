using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraController : MonoBehaviour
{

	[Header ("Camear Option")]
	[SerializeField] Transform target;
	[SerializeField] Vector3 cameraDir;

	[Space(10)]
	[SerializeField] private float minDistance;
	[SerializeField] private float maxDistance;
	[SerializeField] private float cameraDist = 3f;
	[SerializeField] private float sensitivity = 100f;
	[SerializeField] private float minXRot = -50f;
	[SerializeField] private float maxXRot = 50f;
	 
	private float rotationX = 0f;
	private Vector2 mouseDir;

	private void Awake()
	{ 
		InputManager.Instance.MouseMove.action.performed += MouseInput; 
		InputManager.Instance.MouseWheel.action.performed += MouseWheeInput;
	}

	private void LateUpdate()
	{
		if (GameManager.Instance.IsGameOver)
		{ 
			LookAtTarget(); 
			return; 
		}

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

		// 좌우 회전 (Y축 회전)
		transform.Rotate(Vector3.up * mouseDelta.x);
		// 상하 회전 (X축 회전) 
		rotationX -= mouseDelta.y;
		rotationX = Mathf.Clamp(rotationX, minXRot, maxXRot); // 카메라가 뒤집히지 않도록 제한

		// Quaternion을 사용하여 회전 적용
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


	void LookAtTarget()
	{
		Vector3 dir = (target.position - transform.position).normalized;
		var rotation = Quaternion.LookRotation(dir);
		transform.rotation = rotation;
	}
} 
