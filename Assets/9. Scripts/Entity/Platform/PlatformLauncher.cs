using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformLauncher : MovingPlatformController
{
	[SerializeField] private float Maxheight = 30f;

	bool isWaiting = false;
	bool isLaunched = false;
	bool isChagingGauge = false;
	float gauge = 0f;

	private Rigidbody platformRb;

	private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 targetPos;
	private float height;
	private float duration = 3.0f;
	private float timeElapsed;

	private void Start()
	{
		if (platform.GetComponent<Rigidbody>() == null)
		{
			platformRb = platform.AddComponent<Rigidbody>();
			platformRb.isKinematic = true;
		}

		InitPlatformLauncher();

		startPos = positions[0];
		endPos = positions[1];
	}

	private void Update()
	{
		if (isChagingGauge && !isWaiting)
		{
			gauge = Mathf.Min(gauge + Time.deltaTime, 1.0f);
			UIHandler.Instance.GaugeUI.UpdateGauge(gauge); 
		}
	}
	 
	void DropPlatform()
	{

		platformRb.isKinematic = false;
		isLaunched = false;
		isWaiting = true;


		Invoke(nameof(InitPlatformLauncher), 3.0f);

		platformRb.velocity = Vector3.down * ((targetPos - startPos).magnitude / (duration )); 
		List<GameObject> list = new List<GameObject>();
		foreach (var target in targets)
			list.Add(target);

		foreach (var target in list)
			ExitObject(target);
		

	}  
	 
	void InitPlatformLauncher()
	{
		GetComponent<BoxCollider>().enabled = true;
		platformRb.isKinematic = true;
		isChagingGauge = false;
		isLaunched = false;
		isWaiting = false;
		gauge = 0f;
		UIHandler.Instance.GaugeUI.CloseUI();
		 
		InitPlatform();
		
	}

	public override void EnterObject(GameObject go)
	{
		if (isWaiting)
			return;

		base.EnterObject(go);
		if (go.GetComponent<PlayerController>() != null)
		{
			InputManager.Instance.Interaction.action.started += ChargeGauge;
			InputManager.Instance.Interaction.action.canceled += CancelGaugeCharge;
			
		}
	}
	public override void ExitObject(GameObject go)
	{
		base.ExitObject(go);
		if (go.GetComponent<PlayerController>() != null)
		{
			InputManager.Instance.Interaction.action.started -= ChargeGauge;
			InputManager.Instance.Interaction.action.canceled -= CancelGaugeCharge;

			UIHandler.Instance.GaugeUI.CloseUI();

			if (!isLaunched)
			{
				GetComponent<BoxCollider>().enabled = true;
				isChagingGauge = false;  
				gauge = 0f;
			}
		} 
	}


	void ChargeGauge(InputAction.CallbackContext context)
	{
		isChagingGauge = true;
		UIHandler.Instance.GaugeUI.OpenUI();
		GetComponent<BoxCollider>().enabled = false;

	}

	void CancelGaugeCharge(InputAction.CallbackContext context)
	{
		UIHandler.Instance.GaugeUI.CloseUI();
		isChagingGauge = false;
		isLaunched = true;

		Vector3 dir = (endPos - startPos);
		targetPos = startPos + (dir * gauge);   
		height = Maxheight * gauge;
		timeElapsed = 0.0f;
	}

	protected override void MovePlatform()
	{
		if (!isLaunched) 
			return;

		timeElapsed += Time.deltaTime;
		float t = timeElapsed / duration;

		if (t > 1.0f)
		{
			t = 1.0f;
		}
		 
		Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
		currentPos.y += Mathf.Sin(t * Mathf.PI) * height;

		platform.transform.position = currentPos;

		if (t >= 1.0f) 
			DropPlatform();
	}
}
