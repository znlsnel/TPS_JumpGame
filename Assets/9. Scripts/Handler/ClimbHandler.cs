using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class ClimbHandler : MonoBehaviour
{
	[SerializeField] LayerMask climbLayer;

	private Transform rayCastPoint;
	private Transform handTf;

	private Transform climbTargetPos;

	private bool isJump = false;
	private bool isMove = false;

	private CapsuleCollider myCollider;
	private AnimationHandler animationHandler;
	private Rigidbody rigid;
	private void Awake()
	{
		myCollider  = GetComponent<CapsuleCollider>();
		animationHandler = GetComponent<AnimationHandler>();
		rigid = GetComponent<Rigidbody>();

		handTf = Util.FindChildByName(transform, "hand_r"); 
		rayCastPoint = Util.FindChildByName(transform, "ClimbRayCastPoint");
	//	rayCastPoint = handTf; 

		InvokeRepeating(nameof(ClimbCheck), 0, 0.05f);  
	} 
	 
	public void Move(bool active) => isMove = active;
	public void IsJump(bool active) => isJump = active;

	private void FixedUpdate()
	{
		Climb(climbTargetPos);

	}

	void ClimbCheck()
	{
		if (!isJump || !isMove || climbTargetPos != null)
			return;

		for (int i = 0; i < 4; i ++) 
		{ 
			Vector3 yOffset = new Vector3(0, -0.5f * i, 0);
			Ray ray = new Ray(rayCastPoint.position + yOffset, gameObject.transform.forward);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 1.5f, climbLayer)) 
			{
				BoxCollider bc = hit.collider as BoxCollider;
				if (bc != null)
				{
					Vector3 targetPos = hit.point;
					targetPos.y = hit.collider.gameObject.transform.position.y + bc.center.y + (bc.size.y * bc.transform.localScale.y) / 2;

					bool isForward = Vector3.Dot(rigid.velocity, (targetPos - transform.position).normalized) > -0.5f;

					Debug.Log((hit.point.y + -0.5f * i) - targetPos.y);
					if (isForward && Mathf.Abs((rayCastPoint.position.y) - targetPos.y) < 1.3f)
					{  
						targetPos += (transform.forward * 0.2f);
						StartClimb(hit.collider.gameObject, targetPos);
						break;
					}
				}
			}
		}
	}

	void StartClimb(GameObject go, Vector3 targetPos)
	{
		myCollider.enabled = false;

		rigid.useGravity = false;
		rigid.velocity = Vector3.zero;

		GameObject child = new GameObject("Temp");
		child.transform.SetParent(go.transform);
		child.transform.position = targetPos;

		climbTargetPos = child.transform;

		animationHandler.OnClimb();
		InputManager.Instance.ActiveInputSystem(false);
	}

	void Climb(Transform climbPos)
	{
		if (climbPos == null)
			return;

		//Vector3 dir = climbPos.Value - handTf.position;
		Vector3 dir = climbPos.position - transform.position; 
		  
		float dist = dir.magnitude;  
		if (dist > 0.1f)
			dir = dir * dist * Time.fixedDeltaTime * 3.0f;

		rigid.MovePosition(transform.position + dir); 
	} 

	public void EndClimb()
	{
		myCollider.enabled = true;
		rigid.useGravity = true; 
		Destroy(climbTargetPos.gameObject);
		climbTargetPos = null;
		InputManager.Instance.ActiveInputSystem(true);
	}
}
