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
} 
